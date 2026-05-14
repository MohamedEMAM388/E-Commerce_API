using AutoMapper;
using ECommerce.Domain.Contarct;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.ServicesAbstraction;
using Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class BasketServies : IBasketServies
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketServies(IBasketRepository basketRepository , IMapper mapper) 
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        public async Task<CustomerBasketDTO> CreateOrUpdateBasketAsync(CustomerBasketDTO BasketDto)
        {

            var basketMapped = _mapper.Map<CustomerBasket>(BasketDto);
            var basket = await _basketRepository.CreateOrUpdateBasketAsync(basketMapped);

            return _mapper.Map<CustomerBasketDTO>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId) 
                => await _basketRepository.DeleteBasketAsync(basketId);


        public async Task<CustomerBasketDTO> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            return _mapper.Map<CustomerBasketDTO>(basket);
        }
    }
}
