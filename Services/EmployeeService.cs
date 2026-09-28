using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApplication5.Services;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employees;

        public EmployeeService(IEmployeeRepository employees)
        {
            _employees = employees;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employees.GetAll().OrderBy(e => e.Id).ToList();
        }

        public Employee GetById(int id)
        {
            var e = _employees.GetById(id);
            if (e == null)
                throw new ApiException(HttpStatusCode.NotFound, "Сотрудник с таким айди=" + id + " не найден");
            return e;
        }

        public Employee Create(Employee employee)
        {
            if (employee == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нет");

            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ApiException(HttpStatusCode.BadRequest, "ФИО не может быть пустым!");

            if (string.IsNullOrWhiteSpace(employee.Email))
                throw new ApiException(HttpStatusCode.BadRequest, "емаил не может быть пустым!");

            var email = employee.Email.Trim();
            var dup = _employees.GetAll().Any(e =>
                string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));

            if (dup)
                throw new ApiException(HttpStatusCode.BadRequest,"Сотрудник с емаил =" + email + "= уже существует!");

            var newEmp = new Employee
            {
                FullName = employee.FullName.Trim(),
                Department = employee.Department == null ? null : employee.Department.Trim(),
                Email = email
            };

            _employees.Add(newEmp);
            return newEmp;
        }

        public Employee Update(int id, Employee employee)
        {
            var existing = _employees.GetById(id);
            if (existing == null)
                throw new ApiException(HttpStatusCode.NotFound, "Сотрудник с таким айди =" + id + " не найден.");

            if (employee == null)
                throw new ApiException(HttpStatusCode.BadRequest, "тут ниче нет");

            if (string.IsNullOrWhiteSpace(employee.FullName))
                throw new ApiException(HttpStatusCode.BadRequest, "ФИО не может быть пустым!");

            if (string.IsNullOrWhiteSpace(employee.Email))
                throw new ApiException(HttpStatusCode.BadRequest, "емаил не может быть пустым!");

            var email = employee.Email.Trim();
            var dup = _employees.GetAll().Any(e =>
                e.Id != id && string.Equals(e.Email, email, StringComparison.OrdinalIgnoreCase));

            if (dup)
                throw new ApiException(HttpStatusCode.BadRequest,"Сотрудник с емаил =" + email + "= уже существует");

            existing.FullName = employee.FullName.Trim();
            existing.Department = employee.Department == null ? null : employee.Department.Trim();
            existing.Email = email;

            _employees.Update(existing);
            return existing;
        }
    }
}