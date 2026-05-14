using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOS.BasketDTOS
{
    public record BasketItemDTO(
        
         int Id ,
         string Name ,
         string PictureUrl,
         [Range(0 , double.MaxValue)] decimal Price,
         [Range(0 , 100)]   int Quantity
        );
   
}
