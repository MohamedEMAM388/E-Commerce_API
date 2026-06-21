using AutoMapper;
using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.Product;
using ECommerce.ServicesAbstraction;
using Microsoft.Extensions.Configuration;
using Shared.CommonResponses;
using Shared.DTOS.BasketDTOS;
using Shared.DTOS.OrderDTOS;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = ECommerce.Domain.Entities.Product.Product;

namespace ECommerce.Services
{
    public class PaymentServies : IPaymentServies
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IunitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentServies(IBasketRepository basketRepository, 
                             IunitOfWork unitOfWork , IConfiguration configuration,
                              IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<Result<CustomerBasketDTO>> CreateOrUpdatePaymentIntentAsync(string basketid)
        {
            // retrieve the stripe secret key from configuration
            var skey = _configuration["Stripe:SKey"];
            if(string.IsNullOrEmpty(skey))
                return Error.Failure("Failed To Obtaine Secret Key");
            StripeConfiguration.ApiKey = skey;

            // retrive basket by its id 
            var basket = await _basketRepository.GetBasketAsync(basketid);
            if(basket is null)
                return Error.NotFound("Basket not found");

            // validate delivery method inside the basket
            if (basket.DeliveryMethodId is null)
                 return Error.Validation("delivery Method Is Not Selected In The Basket");

            // retrieve the delivery method by its id from db
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod , int>()
                                 .GetByIdAsync(basket.DeliveryMethodId.Value);
            if(deliveryMethod is null)
                return Error.NotFound("Delivery Method Not Found");

            // calculate the shipping price based on the delivery method
            basket.ShippingPrice = deliveryMethod.Price;
            foreach (var item in basket.Items)
            { 
                var product = await _unitOfWork.GetRepository<Product , int>().GetByIdAsync(item.Id);
                if(product is null)
                    return Error.NotFound($"Product notfound");

                item.Price = product.Price;
                item.Name = product.Name;
                item.PictureUrl = product.PictureUrl;
            }

            long amout = (long)(basket.Items.Sum(x => x.Price * x.Quantity) * 100);

            // create or update paymentintent with the calculated amount
            // if payment intent id is null create new payment intent
            var stripeServies = new PaymentIntentService();
            if (basket.PaymentIntentId is null)
            {

                var options = new PaymentIntentCreateOptions()
                {

                    Amount = amout,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]

                };
                var paymentintent = await stripeServies.CreateAsync(options);
                basket.PaymentIntentId = paymentintent.Id;
                basket.ClientSecret = paymentintent.ClientSecret;
            }
            // else update the existing one
            else
            {

                var options = new PaymentIntentUpdateOptions() {
                
                    Amount = amout

                };
                await stripeServies.UpdateAsync(basket.PaymentIntentId, options);

            
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<CustomerBasketDTO>(basket);

        }
    }
}
