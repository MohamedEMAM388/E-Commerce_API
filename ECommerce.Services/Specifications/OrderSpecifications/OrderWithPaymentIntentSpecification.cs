using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Specifications.OrderSpecifications
{
    internal class OrderWithPaymentIntentSpecification : BaseSpecification<Order , Guid>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntent) :
                                                  base(x => x.PaymentIntent == paymentIntent)
        { 
        
        }
    }
}
