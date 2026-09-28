using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication5.Services;
using WebApplication6.Dtos;
using WebApplication6.Mappings;
using WebApplication6.Models;
using WebApplication6.Services;

namespace WebApplication6.Controllers
{
    [RoutePrefix("api/bookings")]
    public class BookingsController : ApiController
    {
        private readonly IBookingService _service;

        public BookingsController()
        {
            _service = ServiceFactory.CreateBookingService();
        }

        // GET /api/bookings?employeeId=&roomId=&date=&status=
        [HttpGet, Route("")]
        public IEnumerable<BookingDto> GetAll(
            [FromUri] int? employeeId = null,
            [FromUri] int? roomId = null,
            [FromUri] DateTime? date = null,
            [FromUri] string status = null)
        {
            return _service.GetAll(employeeId, roomId, date, status).Select(ToDto);
        }

        [HttpGet, Route("{id:int}")]
        public BookingDto GetById(int id)
        {
            return ToDto(_service.GetById(id));
        }

        [HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] CreateBookingDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                EmployeeId = dto.EmployeeId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ParticipantsCount = dto.ParticipantsCount,
                Topic = dto.Topic
            };

            var created = _service.Create(booking);

            return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.Created,
                ToDto(created)));
        }

        [HttpPut, Route("{id:int}")]
        public BookingDto Update(int id, [FromBody] UpdateBookingDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var booking = new Booking
            {
                RoomId = dto.RoomId,
                EmployeeId = dto.EmployeeId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                ParticipantsCount = dto.ParticipantsCount,
                Topic = dto.Topic
            };

            return ToDto(_service.Update(id, booking));
        }

        [HttpDelete, Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _service.Delete(id);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        [HttpPut, Route("{id:int}/status")]
        public BookingDto ChangeStatus(int id, [FromBody] ChangeStatusDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            return ToDto(_service.ChangeStatus(id, dto.Status));
        }

        [HttpGet, Route("{id:int}/history")]
        public IEnumerable<BookingLogDto> History(int id)
        {
            return _service.GetHistory(id).Select(l => l.ToDto());
        }
        private BookingDto ToDto(Booking b)
        {
            var roomRepo = new Infrastructure.MemoryRoomRepository();
            var empRepo = new Infrastructure.MemoryEmployeeRepository();
            var room = roomRepo.GetById(b.RoomId);
            var emp = empRepo.GetById(b.EmployeeId);
            return b.ToDto(room, emp);
        }
    }
}