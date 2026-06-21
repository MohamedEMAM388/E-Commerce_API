using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.OrderModule
{
    public class ItemsOrder : BaseEbtity<int>
    {

        public ProductItemOrder product { get; set; } = default!;
        public decimal price { get; set; }
        public int Quantity { get; set; }
    }
}
