using System.Collections.Generic;
using WebApplication6.Models;

namespace WebApplication6.Services
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        void Add(Employee employee);
        void Update(Employee employee);
    }
}