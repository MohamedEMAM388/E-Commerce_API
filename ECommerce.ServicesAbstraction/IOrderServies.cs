using Shared.CommonResponses;
using Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.ServicesAbstraction
{
    public interface IOrderServies
    {

        // create order => take orderDTO and user email and return OrderToReturnDTO
        // will take email from token so must user be authenticated

        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO , string email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync();

        // get all orders for a user by email and return list of OrderToReturnDTO
        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email);

        // get order by id and email and return OrderToReturnDTO
        Task<Result<OrderToReturnDTO>> GetOrderAsync(Guid id , string email);


    }
}
