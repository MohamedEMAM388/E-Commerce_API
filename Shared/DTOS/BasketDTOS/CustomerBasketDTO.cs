using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BasketDTOS
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; } = default!;
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public List<BasketItemDTO> Items { get; set; } = [];
    }
}
