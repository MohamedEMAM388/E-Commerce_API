using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.Product
{
    public class ProductBrand : BaseEbtity<int>
    {
        public string Name { get; set; } = null!;
    }
}
