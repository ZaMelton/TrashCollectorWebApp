using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrashCollectorV2.Contracts
{
    public interface IRepositoryWrapper
    {
        IEmployeeRepository Employee { get; }
        ICustomerRepository Customer { get; }
        IAddressRepository Address { get; }
        IAccountRepository Account { get; }
        IPickupRepository Pickup { get; }
        void Save();
    }
}
