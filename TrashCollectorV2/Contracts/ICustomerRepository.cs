using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Contracts
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Customer GetCustomer(int customerId);
        void CreateCustomer(Customer customer);

        public List<Customer> GetCustomersIncludeAll();
    }
}
