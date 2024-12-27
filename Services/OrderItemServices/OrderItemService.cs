using AutoMapper;
using Data.DTOs.Order;
using Data.DTOs.OrderItem;
using Repositories.OrderItemRepos;
using Repositories.OrderRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderItemServices
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderItemResponseDTO>> GetAllOrderItemsAsync()
        {
            var orderItems = await _orderItemRepository.GetAllOrderItemAsync();

            // Sử dụng AutoMapper để ánh xạ
            return _mapper.Map<IEnumerable<OrderItemResponseDTO>>(orderItems);
        }
    }
}
