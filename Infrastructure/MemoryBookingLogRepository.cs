using System.Collections.Generic;
using System.Linq;
using WebApplication6.Models;
using WebApplication6.Services;

namespace WebApplication6.Infrastructure
{
    public class MemoryBookingLogRepository : IBookingLogRepository
    {
        private static readonly List<BookingLog> _logs = new List<BookingLog>();
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public IEnumerable<BookingLog> GetByBookingId(int bookingId)
        {
            lock (_lock)
            {
                return _logs
                    .Where(l => l.BookingId == bookingId)
                    .OrderBy(l => l.CreatedAt)
                    .ToList();
            }
        }

        public void Add(BookingLog log)
        {
            lock (_lock)
            {
                log.Id = _nextId++;
                _logs.Add(log);
            }
        }
    }
}