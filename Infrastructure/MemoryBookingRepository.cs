using System.Collections.Generic;
using System.Linq;
using WebApplication6.Models;
using WebApplication6.Services;

namespace WebApplication6.Infrastructure
{
    public class MemoryBookingRepository : IBookingRepository
    {
        private static readonly List<Booking> _bookings = new List<Booking>();
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public IEnumerable<Booking> GetAll()
        {
            lock (_lock) { return _bookings.ToList(); }
        }

        public Booking GetById(int id)
        {
            lock (_lock) { return _bookings.FirstOrDefault(b => b.Id == id); }
        }

        public void Add(Booking booking)
        {
            lock (_lock)
            {
                booking.Id = _nextId++;
                _bookings.Add(booking);
            }
        }

        public void Update(Booking booking)
        {
            lock (_lock)
            {
                var existing = _bookings.FirstOrDefault(b => b.Id == booking.Id);
                if (existing == null) return;
                existing.RoomId = booking.RoomId;
                existing.EmployeeId = booking.EmployeeId;
                existing.StartTime = booking.StartTime;
                existing.EndTime = booking.EndTime;
                existing.ParticipantsCount = booking.ParticipantsCount;
                existing.Topic = booking.Topic;
                existing.Status = booking.Status;
            }
        }

        public void Delete(int id)
        {
            lock (_lock)
            {
                var existing = _bookings.FirstOrDefault(b => b.Id == id);
                if (existing != null) _bookings.Remove(existing);
            }
        }
    }
}