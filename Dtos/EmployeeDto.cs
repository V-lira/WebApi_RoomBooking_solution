using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
    }
}