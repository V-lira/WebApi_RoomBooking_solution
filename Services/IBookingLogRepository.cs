using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IBookingLogRepository
    {
        IEnumerable<BookingLog> GetByBookingId(int bookingId);
        void Add(BookingLog log);
    }
}