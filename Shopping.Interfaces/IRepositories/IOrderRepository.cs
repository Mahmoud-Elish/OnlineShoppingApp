using Shopping.DAL;
using Shopping.Shared;

namespace Shopping.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<OrderDto> GetOrderAsync(int orderId, string userId);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
    Task<ResultDto> CloseOrderAsync(int orderId);
    Task<ResultDto> CreateOrderAsync(string userId, AddOrderDto orderDto);
}
