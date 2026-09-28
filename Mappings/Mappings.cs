using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication6.Dtos;
using WebApplication6.Models;

namespace WebApplication6.Mappings
{
    public static class Mappings
    {
        public static RoomDto ToDto(this Room r)
        {
            if (r == null) return null;
            return new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                HasProjector = r.HasProjector,
                HasWhiteboard = r.HasWhiteboard,
                IsActive = r.IsActive
            };
        }

        public static EmployeeDto ToDto(this Employee e)
        {
            if (e == null) return null;
            return new EmployeeDto
            {
                Id = e.Id,
                FullName = e.FullName,
                Department = e.Department,
                Email = e.Email
            };
        }

        public static BookingDto ToDto(this Booking b, Room room, Employee employee)
        {
            if (b == null) return null;
            return new BookingDto
            {
                Id = b.Id,
                RoomId = b.RoomId,
                RoomName = room == null ? null : room.Name,
                EmployeeId = b.EmployeeId,
                EmployeeFullName = employee == null ? null : employee.FullName,
                Topic = b.Topic,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                ParticipantsCount = b.ParticipantsCount,
                Status = b.Status.ToString()
            };
        }

        public static BookingLogDto ToDto(this BookingLog log)
        {
            if (log == null) return null;
            return new BookingLogDto
            {
                Id = log.Id,
                BookingId = log.BookingId,
                CreatedAt = log.CreatedAt,
                Action = log.Action,
                Details = log.Details
            };
        }
    }
}