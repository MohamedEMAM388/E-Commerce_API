using AutoMapper;
using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.Product;
using ECommerce.Services.Specifications.OrderSpecifications;
using ECommerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.CommonResponses;
using Shared.DTOS.OrderDTOS;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ECommerce.Services
{
    public class OrderServies : IOrderServies
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IunitOfWork _unitOfWork;

        public OrderServies(IMapper mapper ,
            IBasketRepository basketRepository , IunitOfWork unitOfWork )
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            //1-Maps the provided shipping address to the order address entity.
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            //2-Retrieves the basket and validates its existence.
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if(basket is null)
                return Error.NotFound("Basket Not Found", 
                    $"Basket With {orderDTO.BasketId} Was Not Found");
            //3-Creates a list of order items by fetching product details from
            //the database and validating each product.
            List<ItemsOrder> OrderItems = new List<ItemsOrder>();
            foreach (var item in basket.Items)
            {

                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound("Product Not Found",
                          $"Product With Id {item.Id} Was Not Found");
                OrderItems.Add(CreateOrderItem(item, product));

            }

            //4-Retrieves the selected delivery method and validates its existence.
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                       .GetByIdAsync(orderDTO.DeliveryMethodId);
            // validate on delivery method
            if(deliveryMethod is null)
                  return Error.NotFound("Delivery Method Not Found",
                         $"Delivery Method With Id {orderDTO.DeliveryMethodId} Was Not Found");

            //5-Calculates the subtotal of the order based on the items and their quantities.
            var subTotal = OrderItems.Sum(st => st.price * st.Quantity);

            var Orderspec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId!);
            var orderWithPaymentIntent = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Orderspec);

            if (orderWithPaymentIntent is not null)
                _unitOfWork.GetRepository<Order, Guid>().Delete(orderWithPaymentIntent);

            //6-Creates a new Order with all relevant details.
            var order = new Order() 
            { 
                  UserEmail = email,
                  Address = orderAddress,
                  PaymentIntent = basket.PaymentIntentId!,
                  DeliveryMethod = deliveryMethod,
                  SubTotal = subTotal,
                  Items = OrderItems,

            };

             await _unitOfWork.GetRepository<Order , Guid>().AddAsync(order);
            bool result = await _unitOfWork.SaveChangesAsync() > 0;
            if(!result)
                return Error.Failure(
                    "Order Creation Failed", 
                    "An Error Occured While Creating The Order"
                );
            //7-Returns a DTO containing the full order details to the client, including Id[OrderId], UserEmail, items[ProductName, PictureUrl, Price, Quantity],
            //    address, delivery method[ShortName], order status, OrderDate, subtotal, and total price

            return _mapper.Map<OrderToReturnDTO>(order);
        }
        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodAsync()
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if(!deliveryMethods.Any())
                return Error.NotFound("Delivery Methods Not Found", "No Delivery Methods Were Found");
        
            var data = _mapper.Map<IEnumerable<DeliveryMethodDTO>>(deliveryMethods);
            if(data is  null)
                return Error.NotFound("Mapping Error", "Error Occured While Mapping Delivery Methods To DTOs");
       
            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
         
            var spec = new OrderSpicification(email);

            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(spec);
            if(!orders.Any())
                return Error.NotFound(
                    "Orders Not Found", 
                    $"No Orders Were Found For User With Email {email}"
                );

            var data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(data);

        }

        public async Task<Result<OrderToReturnDTO>> GetOrderAsync(Guid id, string email)
        {
            var spec  = new OrderSpicification(email, id);
            var order =  await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(spec);
        
            if(order is null)
                return Error.NotFound(
                    "Order Not Found", 
                    $"No Order Was Found For User With Email {email} And Order Id {id}"
                );

            var data = _mapper.Map<OrderToReturnDTO>(order);
            return Result<OrderToReturnDTO>.Ok(data);
        }

        private static ItemsOrder CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product product)
        {
            return new ItemsOrder()
            {

                product = new ProductItemOrder()
                {

                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl

                },
                price = product.Price,
                Quantity = item.Quantity,

            };
        }


    }
}
