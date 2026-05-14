using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    internal class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEbtity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CoutnDataAsync(ISpecification<TEntity, TKey> specification)
        {
            return await SpecificationEvaluator
                         .CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
       
        }

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
       

        public async Task<IEnumerable<TEntity>> GetAllAsync() => 
               await _dbContext.Set<TEntity>().ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
        {
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification);

            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id) => 
              await _dbContext.Set<TEntity>().FindAsync(id);

        public Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
        {
            var query = SpecificationEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification);
            return query.FirstOrDefaultAsync();
        }

        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
        
    }
}
