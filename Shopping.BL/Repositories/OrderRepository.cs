using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shopping.DAL;
using Shopping.Interfaces;
using Shopping.Shared;

namespace Shopping.BL;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;
    public OrderRepository(AppDbContext context, IConfiguration configuration, ICacheService cacheService) : base(context)
    {
        _configuration = configuration;
        _cacheService = cacheService;
    }
    public async Task<OrderDto> GetOrderAsync(int orderId, string userId)
    {
        var order = await GetQueryable(orderId).MapToOrderDto(userId).FirstOrDefaultAsync();
        return order;
    }
    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
    {
        var orders = await GetQueryable().MapToOrderDto(userId).ToListAsync();
        return orders;
    }
    public async Task<ResultDto> CloseOrderAsync(int orderId)
    {
        var result = await _dbSet.Where(i => i.Id == orderId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(i => i.Status, OrderStatus.Close));

        return result.ToOperationResult();
    }
    #region CreateOrder
    public async Task<ResultDto> CreateOrderAsync(string userId, AddOrderDto orderDto)
    {
        
        var items = await ValidateAndGetItems(orderDto.Items);
        
        var exchangeRate = await GetExchangeRate(orderDto.CurrencyCode);

        
        var (totalPrice, orderDetails) = CalculateOrderTotals(items, orderDto.Items);
        var (discountValue, promoCode) = GetDiscountDetails(orderDto.DiscountPromoCode);

        if (discountValue > 0)
        {
            totalPrice -= discountValue;
        }

        var order = new Order
        {
            CustomerId = userId,
            RequestDate = DateTime.UtcNow,
            Status = OrderStatus.Open,
            DiscountPromoCode = promoCode,
            DiscountValue = discountValue,
            TotalPrice = totalPrice,
            CurrencyCode = orderDto.CurrencyCode,
            ExchangeRate = exchangeRate,
            ForeignPrice = totalPrice / exchangeRate,
            OrderDetails = orderDetails
        };

        var result = await AddAsync(order);
        return result.ToOperationResult();

        //return MapOrderToDto(order);
    }

    private async Task<List<Item>> ValidateAndGetItems(List<OrderItemDto> orderItems)
    {
        var items = new List<Item>();
        var itemIds = orderItems.Select(x => x.ItemId).Distinct();

        foreach (var itemId in itemIds)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null) break; // item not found

            var orderItem = orderItems.First(x => x.ItemId == itemId);
            if (item.Quantity < orderItem.Quantity) break; // less Quantity

            items.Add(item);
        }

        return items;
    }

    private async Task<decimal> GetExchangeRate(string currencyCode)
    {
        if (currencyCode == _configuration["DefaultCurrency"])
            return 1;

        var rate = await _cacheService.GetAsync<CurrencyExchangeRateDto>(
            $"CURRENCY_{currencyCode}");

        if (rate == null)
        {
            throw new KeyNotFoundException($"Exchange rate not found for currency {currencyCode}");
        }

        return rate.Rate;
    }

    private (decimal totalPrice, List<OrderDetail> details) CalculateOrderTotals(List<Item> items, List<OrderItemDto> orderItems)
    {
        var orderDetails = new List<OrderDetail>();
        decimal totalPrice = 0;

        foreach (var item in items)
        {
            var orderItem = orderItems.First(x => x.ItemId == item.Id);
            var detailTotal = item.Price * orderItem.Quantity;

            orderDetails.Add(new OrderDetail
            {
                ItemId = item.Id,
                ItemPrice = item.Price,
                Quantity = orderItem.Quantity,
                TotalPrice = detailTotal
            });

            totalPrice += detailTotal;
        }

        return (totalPrice, orderDetails);
    }

    private (decimal value, string code) GetDiscountDetails(string promoCode)
    {
        if (string.IsNullOrWhiteSpace(promoCode))
            return (0, null);

        var configCode = _configuration["DiscountPromoCode:Code"];
        if (promoCode == configCode)
        {
            return (_configuration.GetValue<decimal>("DiscountPromoCode:Value"), promoCode);
        }

        return (0, null);
    }
    #endregion

}
