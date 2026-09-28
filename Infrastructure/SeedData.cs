using System;
using WebApplication6.Models;

namespace WebApplication6.Infrastructure
{
    public static class SeedData
    {
        private static bool _seeded = false;
        private static readonly object _lock = new object();

        public static void EnsureSeeded()
        {
            lock (_lock)
            {
                if (_seeded) return;
                _seeded = true;

                var rooms = new MemoryRoomRepository();
                var employees = new MemoryEmployeeRepository();
                var bookings = new MemoryBookingRepository();

                //6 и минус одна комнаты
                rooms.Add(new Room { Name = "первая", Capacity = 4, HasProjector = false, HasWhiteboard = true, IsActive = true });
                rooms.Add(new Room { Name = "вторая", Capacity = 8, HasProjector = true, HasWhiteboard = true, IsActive = true });
                rooms.Add(new Room { Name = "третья", Capacity = 12, HasProjector = true, HasWhiteboard = false, IsActive = true });
                rooms.Add(new Room { Name = "четвертая", Capacity = 6, HasProjector = false, HasWhiteboard = false, IsActive = true });
                rooms.Add(new Room { Name = "пятая", Capacity = 20, HasProjector = true, HasWhiteboard = true, IsActive = true });
                rooms.Add(new Room { Name = "ремонтная", Capacity = 10, HasProjector = false, HasWhiteboard = false, IsActive = false });

                //8 работников
                employees.Add(new Employee { FullName = "Иванов Иван Иванович", Department = "IT", Email = "ivanov@au.com" });
                employees.Add(new Employee { FullName = "Петров Петр Петрович", Department = "Бухгалтерия", Email = "petrovICH@au.com" });
                employees.Add(new Employee { FullName = "Сергеев Сергей Сегреевич", Department = "IT", Email = "segrehrc@au.com" });
                employees.Add(new Employee { FullName = "Олегова Ольга Олеговна", Department = "HR", Email = "oltggfkd@au.com" });
                employees.Add(new Employee { FullName = "Дмитриев Дмитрий Дмитриевич", Department = "Продажи", Email = "fdfskgm@au.com" });
                employees.Add(new Employee { FullName = "Викторов Вкитор Викторович", Department = "Маркетинг", Email = "r8h4fij@au.com" });
                employees.Add(new Employee { FullName = "Алексеев Алексей Алексеевич", Department = "Продажи", Email = "fihrsjfn@cau.com" });
                employees.Add(new Employee { FullName = "Фёдоров Фёдор Фёдорович", Department = "HR", Email = "fedorovPRO@au.com" });

                var today = DateTime.Today;
                //////////////////////////////////////////////////////////////////////////////////////
                //10с разными статусами и датами
                //планы (5)
                bookings.Add(new Booking
                {
                    RoomId = 1,
                    EmployeeId = 1,
                    StartTime = today.AddDays(1).AddHours(10),
                    EndTime = today.AddDays(1).AddHours(11),
                    ParticipantsCount = 3,
                    Topic = "Планёрка отдела",
                    Status = BookingStatus.Planned
                });

                bookings.Add(new Booking
                {
                    RoomId = 2,
                    EmployeeId = 2,
                    StartTime = today.AddDays(1).AddHours(14),
                    EndTime = today.AddDays(1).AddHours(15),
                    ParticipantsCount = 6,
                    Topic = "Согласование бюджета",
                    Status = BookingStatus.Planned
                });

                bookings.Add(new Booking
                {
                    RoomId = 3,
                    EmployeeId = 3,
                    StartTime = today.AddDays(2).AddHours(9),
                    EndTime = today.AddDays(2).AddHours(11),
                    ParticipantsCount = 10,
                    Topic = "Технический совет",
                    Status = BookingStatus.Planned
                });

                bookings.Add(new Booking
                {
                    RoomId = 5,
                    EmployeeId = 4,
                    StartTime = today.AddDays(3).AddHours(16),
                    EndTime = today.AddDays(3).AddHours(17),
                    ParticipantsCount = 15,
                    Topic = "Общее собрание",
                    Status = BookingStatus.Planned
                });

                bookings.Add(new Booking
                {
                    RoomId = 1,
                    EmployeeId = 2,
                    StartTime = today.AddDays(6).AddHours(9),
                    EndTime = today.AddDays(6).AddHours(10),
                    ParticipantsCount = 3,
                    Topic = "1-на-1 с руководителем",
                    Status = BookingStatus.Planned
                });
                //стартед (2)
                bookings.Add(new Booking
                {
                    RoomId = 4,
                    EmployeeId = 5,
                    StartTime = DateTime.Now.AddMinutes(-30),
                    EndTime = DateTime.Now.AddHours(1),
                    ParticipantsCount = 4,
                    Topic = "Интервью кандидата",
                    Status = BookingStatus.Started
                });

                bookings.Add(new Booking
                {
                    RoomId = 2,
                    EmployeeId = 6,
                    StartTime = DateTime.Now.AddMinutes(-10),
                    EndTime = DateTime.Now.AddMinutes(50),
                    ParticipantsCount = 5,
                    Topic = "Созвон с клиентом",
                    Status = BookingStatus.Started
                });

                //финиш(2)
                bookings.Add(new Booking
                {
                    RoomId = 1,
                    EmployeeId = 7,
                    StartTime = today.AddDays(-2).AddHours(11),
                    EndTime = today.AddDays(-2).AddHours(12),
                    ParticipantsCount = 2,
                    Topic = "Ретроспектива",
                    Status = BookingStatus.Finished
                });

                bookings.Add(new Booking
                {
                    RoomId = 3,
                    EmployeeId = 8,
                    StartTime = today.AddDays(-1).AddHours(15),
                    EndTime = today.AddDays(-1).AddHours(16),
                    ParticipantsCount = 7,
                    Topic = "Демо продукта",
                    Status = BookingStatus.Finished
                });

                //отклонено (1)
                bookings.Add(new Booking
                {
                    RoomId = 5,
                    EmployeeId = 1,
                    StartTime = today.AddDays(4).AddHours(13),
                    EndTime = today.AddDays(4).AddHours(14),
                    ParticipantsCount = 8,
                    Topic = "Тренинг (отменён)",
                    Status = BookingStatus.Cancelled
                });
            }
        }
    }
}