using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contarct
{
    public interface ISpecification<TEntity , TKey> where TEntity : BaseEbtity<TKey>
    {
        // includes 

        ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; }

        // criteria 
        Expression<Func<TEntity, bool>> CriteriaExp { get; }

        // sorting
        Expression<Func<TEntity , object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDescending { get; }

        // pagination
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }


    }
}
