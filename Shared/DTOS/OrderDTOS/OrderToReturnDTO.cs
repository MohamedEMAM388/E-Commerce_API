using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.OrderDTOS
{
    public record OrderToReturnDTO
    {

        public Guid Id { get; init; }
        public string UserEmail { get; init; }
        public ICollection<OrderItemDTO> Items { get; init; }

        //address, delivery method [ShortName], order status, OrderDate ,
        //subtotal, and total price

        public AddressDTO Address { get; init; }
        public string DeliveryMethod { get; init; } // as [short name]
        public string OrderStatus { get; init; }
        public DateTimeOffset OrderDate { get; init; }
        public decimal subtotal { get; init; }
        public decimal Total { get; init; }


    }
}
