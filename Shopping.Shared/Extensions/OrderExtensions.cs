using Shopping.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public static class OrderExtensions
{
    public static IQueryable<OrderDto> MapToOrderDto(this IQueryable<Order> order,string userId)
    {
        return order.Where(o=>o.CustomerId == userId).Select(o=> new OrderDto(
            o.Id,
            o.RequestDate,
            o.CloseDate,
            o.Status.ToString(),
            o.TotalPrice,
            o.CurrencyCode,
            o.ExchangeRate,
            o.ForeignPrice,
            o.OrderDetails.Select(od => new OrderDetailDto(
            od.ItemId,
            od.ItemPrice,
            od.Quantity,
            od.TotalPrice)).ToList()
        ));
    }
}
