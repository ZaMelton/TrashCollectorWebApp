using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollectorV2.Models
{
    public class ViewModel
    {
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public Employee Employee { get; set; }
        public IQueryable<Customer> CustomerList { get; set; }
    }
}
