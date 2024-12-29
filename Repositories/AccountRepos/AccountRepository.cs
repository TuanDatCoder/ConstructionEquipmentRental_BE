using Data.Entities;
using Data;
using Repositories.GenericRepos;
using Repositories.ProductImageRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AccountRepos
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        private readonly ConstructionEquipmentRentalDbContext _context;

        public AccountRepository(ConstructionEquipmentRentalDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
