using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contarct
{
    public interface IunitOfWork
    {

        Task<int> SaveChangesAsync();

        IGenericRepository<TEntity , TKey> GetRepository<TEntity , TKey>() where TEntity : BaseEbtity<TKey>;

    }
}
