using WebApplication6.Infrastructure;

namespace WebApplication6.Services
{
    public static class ServiceFactory
    {
        private static readonly IRoomRepository _rooms = new MemoryRoomRepository();
        private static readonly IEmployeeRepository _employees = new MemoryEmployeeRepository();
        private static readonly IBookingRepository _bookings = new MemoryBookingRepository();
        private static readonly IBookingLogRepository _logs = new MemoryBookingLogRepository();

        public static IRoomService CreateRoomService()
        {
            return new RoomService(_rooms, _bookings);
        }

        public static IEmployeeService CreateEmployeeService()
        {
            return new EmployeeService(_employees);
        }

        public static IBookingService CreateBookingService()
        {
            return new BookingService(_bookings, _rooms, _employees, _logs);
        }
    }
}