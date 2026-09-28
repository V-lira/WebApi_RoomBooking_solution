using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApplication5.Services;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _rooms;
        private readonly IBookingRepository _bookings;

        public RoomService(IRoomRepository rooms, IBookingRepository bookings)
        {
            _rooms = rooms;
            _bookings = bookings;
        }

        public IEnumerable<Room> GetAll()
        {
            return _rooms.GetAll().OrderBy(r => r.Id).ToList();
        }

        public Room GetById(int id)
        {
            var room = _rooms.GetById(id);
            if (room == null)
                throw new ApiException(HttpStatusCode.NotFound, "Комната с таким айди=" + id + " не найдена!");
            return room;
        }

        public Room Create(Room room)
        {
            if (room == null || string.IsNullOrWhiteSpace(room.Name))
                throw new ApiException(HttpStatusCode.BadRequest, "Название комнаты не может быть пустым");

            if (room.Capacity <= 0)
                throw new ApiException(HttpStatusCode.BadRequest, "Вместимость должна быть больше нуля");

            var name = room.Name.Trim();
            var dup = _rooms.GetAll().Any(r =>
                string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));

            if (dup)
                throw new ApiException(HttpStatusCode.BadRequest,"Комната с названием =" + name + "= уже существует!!");

            var newRoom = new Room
            {
                Name = name,
                Capacity = room.Capacity,
                HasProjector = room.HasProjector,
                HasWhiteboard = room.HasWhiteboard,
                IsActive = true
            };

            _rooms.Add(newRoom);
            return newRoom;
        }

        public Room Update(int id, Room room)
        {
            var existing = _rooms.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "Комната с таким айди=" + id + " не найдена");

            if (room == null || string.IsNullOrWhiteSpace(room.Name))
                throw new ApiException(HttpStatusCode.BadRequest, "Название комнаты не может быть пустым");

            if (room.Capacity <= 0)
                throw new ApiException(HttpStatusCode.BadRequest, "Вместимость должна быть больше нуля");

            var name = room.Name.Trim();
            var dup = _rooms.GetAll().Any(r =>
                r.Id != id && string.Equals(r.Name, name, StringComparison.OrdinalIgnoreCase));

            if (dup)
                throw new ApiException(HttpStatusCode.BadRequest,"Комната с названием =" + name + "= уже существует.");

            existing.Name = name;
            existing.Capacity = room.Capacity;
            existing.HasProjector = room.HasProjector;
            existing.HasWhiteboard = room.HasWhiteboard;

            _rooms.Update(existing);
            return existing;
        }

        public Room Deactivate(int id)
        {
            var existing = _rooms.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "Комната с таким айди=" + id + " не найдена");

            if (!existing.IsActive)
                throw new ApiException(HttpStatusCode.Conflict, "увы, не активно");

            existing.IsActive = false;
            _rooms.Update(existing);
            return existing;
        }

        public IEnumerable<Room> FindAvailable(DateTime start, DateTime end, int participants, bool? projector, bool? whiteboard)
        {
            if (end <= start)
                throw new ApiException(HttpStatusCode.BadRequest, "Конец интервала должен быть позже начала!");
            if (participants <= 0)
                throw new ApiException(HttpStatusCode.BadRequest, "колво участников должно быть больше нуля!");

            var activeBookings = _bookings.GetAll()
                .Where(b => b.Status == BookingStatus.Planned || b.Status == BookingStatus.Started)
                .ToList();

            var result = new List<Room>();

            foreach (var room in _rooms.GetAll())
            {
                if (!room.IsActive) continue;
                if (room.Capacity < participants) continue;
                if (projector == true && !room.HasProjector) continue;
                if (whiteboard == true && !room.HasWhiteboard) continue;

                bool overlaps = activeBookings.Any(b =>
                    b.RoomId == room.Id &&
                    start < b.EndTime && b.StartTime < end);

                if (!overlaps) result.Add(room);
            }

            return result;
        }
    }
}