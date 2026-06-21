using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation
{
    public class OrderController : ApiBaseController
    {
        private readonly IOrderServies _orderServies;

        public OrderController(IOrderServies orderServies)
        {
            _orderServies = orderServies;
        }

        // create order
        [Authorize]
        [HttpPost]

        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDto)
        {

            var result = await _orderServies.CreateOrderAsync(orderDto, GeEmailFromTokenClaims());
            return HandleResult(result);
        }

        //get all orders for a user by email
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {

            var result = await _orderServies.GetAllOrdersAsync(GeEmailFromTokenClaims());
            return HandleResult(result);
        }

        // get specific order for specific user
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {

            var result = await _orderServies.GetOrderAsync(id, GeEmailFromTokenClaims());
            return HandleResult(result);
        }
        // get all delivery method
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<DeliveryMethodDTO>> GetDeliveryMethods()
        {

            var deliverymethod = await _orderServies.GetAllDeliveryMethodAsync();
            return HandleResult(deliverymethod);
        
        }


    }
}
