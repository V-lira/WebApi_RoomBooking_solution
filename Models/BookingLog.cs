using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class BookingLog
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
    }
}