using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications
{
    internal class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEbtity<TKey>
    {
        public BaseSpecification(Expression<Func<TEntity , bool>> criteria)
        {
            CriteriaExp = criteria;
        }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        public Expression<Func<TEntity, bool>> CriteriaExp { get; }

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplayPagination(int pagesize, int pageindex)
        {

            IsPaginated = true;
            Skip = (pageindex - 1) * pagesize;
            Take = pagesize;
        } 
        #endregion

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderbyexp) { 
        
            OrderBy = orderbyexp;
        }

        protected void AddOrderByDescinding(Expression<Func<TEntity, object>> orderbydescexp) { 
        
            OrderByDescending = orderbydescexp;
        }
    }
}
