using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrashCollectorV2.Contracts;
using TrashCollectorV2.Models;

namespace TrashCollectorV2.Data
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public Account GetAccount(int accountId) => FindByCondition(c => c.Id == accountId).SingleOrDefault();
    }
}
