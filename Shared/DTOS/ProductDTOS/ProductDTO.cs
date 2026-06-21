using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.ProductDTOS
{
    public record ProductDTO
    {
        public int Id { get; init; }

        public string Name { get; init; } = default!;

        public string Description { get; init; } = default!;

        public string PictureUrl { get; init; } = default!;

        public decimal Price { get; init; }

        public string ProductBrand { get; init; } = default!;   

        public string ProductType { get; init; } = default!;
    }
}
