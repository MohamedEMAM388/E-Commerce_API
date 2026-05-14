using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class UnitOfWork : IunitOfWork
    {
        private readonly StoreDbContext _dbContext;

        // we can use a dictionary to store the repositories instances and
        // return the same instance when requested again
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEbtity<TKey>
        {
            var tentity = typeof(TEntity);

            if (_repositories.TryGetValue(tentity, out var repository)) {

                return (IGenericRepository<TEntity , TKey>)repository;
            }

            var newrepository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories.Add(tentity, newrepository);
            return newrepository;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();
        
    }
}
