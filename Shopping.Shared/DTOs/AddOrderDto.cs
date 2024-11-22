using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public class AddOrderDto
{
    public List<OrderItemDto> Items { get; set; }
    public string CurrencyCode { get; set; }
    public string DiscountPromoCode { get; set; }
}
