﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Data
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Customer GetCustomer(int customerId) => FindByCondition(c => c.Id == customerId).SingleOrDefault();

        public void CreateCustomer(Customer customer) => Create(customer);

        public List<Customer> GetCustomersIncludeAll() => FindAll().Include(a => a.Address).Include(b => b.Account).ToList();
    }
}
