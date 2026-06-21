using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications.OrderSpecifications
{
    internal class OrderSpicification : BaseSpecification<Order , Guid>
    {

        public OrderSpicification(string email) : base(o => o.UserEmail == email) {

            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDescinding(o => o.OrderDate);

        }
        public OrderSpicification(string email , Guid id) : 
                      base(o => o.UserEmail == email && o.Id == id)
        {

            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDescinding(o => o.OrderDate);

        }
    }
}
