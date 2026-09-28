using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApplication5.Services;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookings;
        private readonly IRoomRepository _rooms;
        private readonly IEmployeeRepository _employees;
        private readonly IBookingLogRepository _logs;

        public BookingService(
            IBookingRepository bookings,
            IRoomRepository rooms,
            IEmployeeRepository employees,
            IBookingLogRepository logs)
        {
            _bookings = bookings;
            _rooms = rooms;
            _employees = employees;
            _logs = logs;
        }

        public IEnumerable<Booking> GetAll(int? employeeId, int? roomId, DateTime? date, string status)
        {
            var q = _bookings.GetAll().AsQueryable();

            if (employeeId.HasValue) q = q.Where(b => b.EmployeeId == employeeId.Value);
            if (roomId.HasValue) q = q.Where(b => b.RoomId == roomId.Value);

            if (date.HasValue)
            {
                var d = date.Value.Date;
                q = q.Where(b => b.StartTime.Date == d);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                BookingStatus parsed;
                if (!TryParseStatus(status, out parsed))
                    throw new ApiException(HttpStatusCode.BadRequest,
                        "странный статус ==" + status + "==");
                q = q.Where(b => b.Status == parsed);
            }

            return q.OrderBy(b => b.StartTime).ToList();
        }

        public Booking GetById(int id)
        {
            var b = _bookings.GetById(id);
            if (b == null)
                throw new ApiException(HttpStatusCode.NotFound, "это с таким айди=" + id + " не найдено!");
            return b;
        }

        public IEnumerable<BookingLog> GetHistory(int id)
        {
            GetById(id);
            return _logs.GetByBookingId(id);
        }

        public Booking Create(Booking booking)
        {
            if (booking == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нет");

            Validate(booking, null);

            var newBooking = new Booking
            {
                RoomId = booking.RoomId,
                EmployeeId = booking.EmployeeId,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                ParticipantsCount = booking.ParticipantsCount,
                Topic = booking.Topic.Trim(),
                Status = BookingStatus.Planned
            };

            _bookings.Add(newBooking);
            WriteLog(newBooking.Id, "Created", "создано бронирование: «" + newBooking.Topic + "».");
            return newBooking;
        }

        public Booking Update(int id, Booking booking)
        {
            var existing = _bookings.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "это с таким айди=" + id + " не найдено");

            if (existing.Status == BookingStatus.Finished || existing.Status == BookingStatus.Cancelled)
                throw new ApiException(HttpStatusCode.Conflict,
                    "Нельзя изменять бронирование со статусом !" + existing.Status + ".");

            if (booking == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нет");

            Validate(booking, id);

            existing.RoomId = booking.RoomId;
            existing.EmployeeId = booking.EmployeeId;
            existing.StartTime = booking.StartTime;
            existing.EndTime = booking.EndTime;
            existing.ParticipantsCount = booking.ParticipantsCount;
            existing.Topic = booking.Topic.Trim();

            _bookings.Update(existing);
            WriteLog(existing.Id, "Updated", "Данные изменены");
            return existing;
        }

        public Booking ChangeStatus(int id, string newStatus)
        {
            var existing = _bookings.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "это с таким айди=" + id + " не найдено.");

            BookingStatus parsed;
            if (!TryParseStatus(newStatus, out parsed))
                throw new ApiException(HttpStatusCode.BadRequest,
                    "Неизвестный статус «" + newStatus + "».");

            if (!IsTransitionAllowed(existing.Status, parsed))
                throw new ApiException(HttpStatusCode.Conflict,
                    "Переход «" + existing.Status + " → " + parsed + "» запрещён.");

            var old = existing.Status;
            existing.Status = parsed;
            _bookings.Update(existing);

            WriteLog(existing.Id, "StatusChanged", "Статус изменён: " + old + " → " + parsed + ".");

            if (parsed == BookingStatus.Cancelled)
                WriteLog(existing.Id, "Cancelled", "Бронирование отменено.");

            return existing;
        }

        public void Delete(int id)
        {
            var existing = _bookings.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "это с таким айди=" + id + " не найдено.");

            if (existing.Status == BookingStatus.Started)
                throw new ApiException(HttpStatusCode.Conflict, "Нельзя удалить Started-бронирование.");

            if (existing.Status == BookingStatus.Finished)
                throw new ApiException(HttpStatusCode.Conflict, "Нельзя удалить Finished-бронирование.");

            WriteLog(existing.Id, "Deleted", "Бронирование удалено.");
            _bookings.Delete(id);
        }

        // ---- helpers ----

        private static bool TryParseStatus(string status, out BookingStatus parsed)
        {
            parsed = BookingStatus.Planned;
            if (string.IsNullOrWhiteSpace(status)) return false;
            return Enum.TryParse(status, ignoreCase: true, result: out parsed)
                   && Enum.IsDefined(typeof(BookingStatus), parsed);
        }

        private static bool IsTransitionAllowed(BookingStatus from, BookingStatus to)
        {
            switch (from)
            {
                case BookingStatus.Planned:
                    return to == BookingStatus.Started || to == BookingStatus.Cancelled;
                case BookingStatus.Started:
                    return to == BookingStatus.Finished;
                default:
                    return false;
            }
        }

        private void Validate(Booking b, int? excludeId)
        {
            var room = _rooms.GetById(b.RoomId);
            if (room == null)
                throw new ApiException(HttpStatusCode.NotFound,"Комната с Id=" + b.RoomId + " не найдена.");

            var emp = _employees.GetById(b.EmployeeId);
            if (emp == null)
                throw new ApiException(HttpStatusCode.NotFound,"Сотрудник с Id=" + b.EmployeeId + " не найден.");

            if (!room.IsActive)
                throw new ApiException(HttpStatusCode.Conflict,"Комната «" + room.Name + "» недоступна для новых бронирований.");

            if (b.StartTime >= b.EndTime)
                throw new ApiException(HttpStatusCode.BadRequest,"Время начала должно быть раньше времени окончания.");

            if (b.StartTime < DateTime.Now)
                throw new ApiException(HttpStatusCode.BadRequest,"Время начала не может быть в прошлом.");

            if (b.ParticipantsCount <= 0)
                throw new ApiException(HttpStatusCode.BadRequest,"Количество участников должно быть больше нуля.");

            if (b.ParticipantsCount > room.Capacity)
                throw new ApiException(HttpStatusCode.Conflict,"Количество участников (" + b.ParticipantsCount +") превышает вместимость комнаты (" + room.Capacity + ").");

            if (string.IsNullOrWhiteSpace(b.Topic))
                throw new ApiException(HttpStatusCode.BadRequest, "Тема встречи не может быть пустой.");

            var active = _bookings.GetAll().Where(x =>
                x.RoomId == b.RoomId &&
                x.Status != BookingStatus.Cancelled &&
                x.Status != BookingStatus.Finished);

            if (excludeId.HasValue)
                active = active.Where(x => x.Id != excludeId.Value);

            var overlap = active.FirstOrDefault(x =>
                b.StartTime < x.EndTime && x.StartTime < b.EndTime);

            if (overlap != null)
                throw new ApiException(HttpStatusCode.Conflict,"Интервал пересекается с существующим бронированием (айди=" + overlap.Id + ").");
        }

        private void WriteLog(int bookingId, string action, string details)
        {
            _logs.Add(new BookingLog
            {
                BookingId = bookingId,
                CreatedAt = DateTime.Now,
                Action = action,
                Details = details
            });
        }
    }
}