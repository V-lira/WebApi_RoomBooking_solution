using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        Employee Create(Employee employee);
        Employee Update(int id, Employee employee);
    }
}