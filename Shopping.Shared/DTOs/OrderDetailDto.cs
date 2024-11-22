using Shopping.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public record OrderDetailDto
(
    int ItemId, decimal ItemPrice, int Quantity, decimal TotalPrice
);
