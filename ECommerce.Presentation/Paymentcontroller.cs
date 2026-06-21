using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{
    public class Paymentcontroller : ApiBaseController
    {
        private readonly IPaymentServies _paymentServies;

        public Paymentcontroller(IPaymentServies paymentServies) 
        {
            _paymentServies = paymentServies;
        }

        // create or update payment intent

        [HttpPost("{Basketid}")]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent(string Basketid)
        {
           var result = await _paymentServies.CreateOrUpdatePaymentIntentAsync(Basketid);
            return HandleResult(result);
        }
    }
}
