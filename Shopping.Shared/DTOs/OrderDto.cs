using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public record OrderDto
(
    int OrderId, DateTime? RequestDate,DateTime? CloseDate, string Status, decimal TotalPrice,
    string? CurrencyCode, decimal? ExchangeRate, decimal? ForeignPrice,
    IReadOnlyList<OrderDetailDto> Details
);
