using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    internal class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> entrypoint, ISpecification<TEntity, TKey> specification) where TEntity : BaseEbtity<TKey>
        {
            var Query = entrypoint;

            if (specification is not null) {

                if (specification.CriteriaExp is not null) {

                    Query = Query.Where(specification.CriteriaExp);
                }
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any()) { 
                
                    Query = specification.IncludeExpression.Aggregate(Query, (current, include) => 
                                                                       current.Include(include));

                }
                if (specification.OrderBy is not null) { 
                
                    Query = Query.OrderBy(specification.OrderBy);
                }
                if (specification.OrderByDescending is not null) { 
                
                    Query = Query.OrderByDescending(specification.OrderByDescending);
                }
                if (specification.IsPaginated == true) {

                    Query = Query.Skip(specification.Skip).Take(specification.Take);
                }
            }

            return Query;
        }
    }
}
