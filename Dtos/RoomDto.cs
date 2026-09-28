using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasProjector { get; set; }
        public bool HasWhiteboard { get; set; }
        public bool IsActive { get; set; }
    }
}