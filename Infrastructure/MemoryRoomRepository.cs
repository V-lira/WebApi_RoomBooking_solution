using System.Collections.Generic;
using System.Linq;
using WebApplication6.Models;
using WebApplication6.Services;

namespace WebApplication6.Infrastructure
{
    public class MemoryRoomRepository : IRoomRepository
    {
        private static readonly List<Room> _rooms = new List<Room>();
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public IEnumerable<Room> GetAll()
        {
            lock (_lock) { return _rooms.ToList(); }
        }

        public Room GetById(int id)
        {
            lock (_lock) { return _rooms.FirstOrDefault(r => r.Id == id); }
        }

        public void Add(Room room)
        {
            lock (_lock)
            {
                room.Id = _nextId++;
                _rooms.Add(room);
            }
        }

        public void Update(Room room)
        {
            lock (_lock)
            {
                var existing = _rooms.FirstOrDefault(r => r.Id == room.Id);
                if (existing == null) return;
                existing.Name = room.Name;
                existing.Capacity = room.Capacity;
                existing.HasProjector = room.HasProjector;
                existing.HasWhiteboard = room.HasWhiteboard;
                existing.IsActive = room.IsActive;
            }
        }
    }
}