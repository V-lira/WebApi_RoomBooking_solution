using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(int id);
    }
}