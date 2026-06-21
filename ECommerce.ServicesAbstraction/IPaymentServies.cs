using Shared.CommonResponses;
using Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IPaymentServies
    {

        // create or update payment intent
        Task<Result<CustomerBasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketid);

        //webhook 
        Task UpdateOrderPaymentStatus(string request, string stripesignture);
    }
}
