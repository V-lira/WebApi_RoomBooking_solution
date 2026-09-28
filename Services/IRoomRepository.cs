using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        void Add(Room room);
        void Update(Room room);
    }
}