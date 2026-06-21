using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.ProductDTOS
{
    public record ProductTypeDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
