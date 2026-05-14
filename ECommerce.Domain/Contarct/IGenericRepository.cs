using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contarct
{
    public interface IGenericRepository<TEntity , TKey> where TEntity : BaseEbtity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification);


        // get by id 
        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification); 

        // add new entity
        Task AddAsync(TEntity entity);

        // update entity
        void Update(TEntity entity);

        // delete entity
        void Delete(TEntity entity);

        // count total of returned data
        Task<int> CoutnDataAsync(ISpecification<TEntity, TKey> specification);
    }
}
