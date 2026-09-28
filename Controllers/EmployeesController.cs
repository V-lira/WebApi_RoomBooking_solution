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
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        private readonly IEmployeeService _service;

        public EmployeesController()
        {
            _service = ServiceFactory.CreateEmployeeService();
        }

        [HttpGet, Route("")]
        public IEnumerable<EmployeeDto> GetAll()
        {
            return _service.GetAll().Select(e => e.ToDto());
        }

        [HttpGet, Route("{id:int}")]
        public EmployeeDto GetById(int id)
        {
            return _service.GetById(id).ToDto();
        }

        [HttpPost, Route("")]
        public IHttpActionResult Create([FromBody] EmployeeDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var emp = new Employee
            {
                FullName = dto.FullName,
                Department = dto.Department,
                Email = dto.Email
            };

            var created = _service.Create(emp);

            return ResponseMessage(Request.CreateResponse(
                HttpStatusCode.Created,
                created.ToDto()));
        }

        [HttpPut, Route("{id:int}")]
        public EmployeeDto Update(int id, [FromBody] EmployeeDto dto)
        {
            if (dto == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нету");

            var emp = new Employee
            {
                FullName = dto.FullName,
                Department = dto.Department,
                Email = dto.Email
            };

            return _service.Update(id, emp).ToDto();
        }
    }
}