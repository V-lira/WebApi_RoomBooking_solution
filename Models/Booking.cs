using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ParticipantsCount { get; set; }
        public string Topic { get; set; }
        public BookingStatus Status { get; set; }
    }
}