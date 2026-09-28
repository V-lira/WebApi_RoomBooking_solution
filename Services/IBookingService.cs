using System;
using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IBookingService
    {
        IEnumerable<Booking> GetAll(int? employeeId, int? roomId, DateTime? date, string status);
        Booking GetById(int id);
        Booking Create(Booking booking);
        Booking Update(int id, Booking booking);
        void Delete(int id);
        Booking ChangeStatus(int id, string newStatus);
        IEnumerable<BookingLog> GetHistory(int id);
    }
}