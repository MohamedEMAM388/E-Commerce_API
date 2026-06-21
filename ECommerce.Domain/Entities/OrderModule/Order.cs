using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.OrderModule
{
    public class Order : BaseEbtity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.pending;
        public string PaymentIntent { get; set; } = default!;
        public OrderAddress Address { get; set; } = default!; // owned entity
        public DeliveryMethod DeliveryMethod { get; set; } = default!; // one to many from delivery MEthod
        public int DeliveryMethodId { get; set; }

        // itemorder => order have many item one to many relation
        public ICollection<ItemsOrder> Items { get; set; } = [];

        public decimal SubTotal { get; set; } // price of product * quantity
        public decimal GetTotal() => SubTotal + DeliveryMethod.Price; // is a read only property



    }
}
