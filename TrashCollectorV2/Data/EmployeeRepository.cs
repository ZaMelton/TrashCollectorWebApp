using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Data
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public void CreateEmployee(Employee employee)
        {
            Create(employee);
        }

        public Employee GetEmployee(int employeeId)
        {
            return FindByCondition(e => e.Id == employeeId).SingleOrDefault();
        }
    }
}
