using System;
using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IRoomService
    {
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        Room Create(Room room);
        Room Update(int id, Room room);
        Room Deactivate(int id);
        IEnumerable<Room> FindAvailable(DateTime start, DateTime end, int participants, bool? projector, bool? whiteboard);
    }
}