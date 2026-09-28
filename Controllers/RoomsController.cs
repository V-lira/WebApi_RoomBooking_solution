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
    [RoutePrefix("api/rooms")]
    public class RoomsController : ApiController
    {
        private readonly IRoomService _service;

        public RoomsController()
        {
            _service = ServiceFactory.CreateRoomService();
        }

        [HttpGet, Route("")]
        public IEnumerable<RoomDto> GetAll()
        {
            return _service.GetAll().Select(r => r.ToDto());
        }

        [HttpGet, Route("{id:int}")]
        public RoomDto GetById(int id)
        {
            return _service.GetById(id).ToDto();
        }

        [HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] RoomDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var room = new Room
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                HasProjector = dto.HasProjector,
                HasWhiteboard = dto.HasWhiteboard
            };

            var created = _service.Create(room);

            return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.Created,
                created.ToDto()));
        }

        [HttpPut, Route("{id:int}")]
        public RoomDto Update(int id, [FromBody] RoomDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var room = new Room
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                HasProjector = dto.HasProjector,
                HasWhiteboard = dto.HasWhiteboard
            };

            return _service.Update(id, room).ToDto();
        }

        [HttpPut, Route("{id:int}/deactivate")]
        public RoomDto Deactivate(int id)
        {
            return _service.Deactivate(id).ToDto();
        }

        [HttpGet, Route("available")]
        public IEnumerable<RoomDto> Available(
            [FromUri] DateTime start,
            [FromUri] DateTime end,
            [FromUri] int participants,
            [FromUri] bool? projector = null,
            [FromUri] bool? whiteboard = null)
        {
            return _service
                .FindAvailable(start, end, participants, projector, whiteboard)
                .Select(r => r.ToDto());
        }
    }
}