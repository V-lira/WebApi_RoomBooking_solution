using System.Collections.Generic;
using System.Linq;
using WebApplication6.Models;
using WebApplication6.Services;

namespace WebApplication6.Infrastructure
{
    public class MemoryEmployeeRepository : IEmployeeRepository
    {
        private static readonly List<Employee> _employees = new List<Employee>();
        private static int _nextId = 1;
        private static readonly object _lock = new object();

        public IEnumerable<Employee> GetAll()
        {
            lock (_lock) { return _employees.ToList(); }
        }

        public Employee GetById(int id)
        {
            lock (_lock) { return _employees.FirstOrDefault(e => e.Id == id); }
        }

        public void Add(Employee employee)
        {
            lock (_lock)
            {
                employee.Id = _nextId++;
                _employees.Add(employee);
            }
        }

        public void Update(Employee employee)
        {
            lock (_lock)
            {
                var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
                if (existing == null) return;
                existing.FullName = employee.FullName;
                existing.Department = employee.Department;
                existing.Email = employee.Email;
            }
        }
    }
}