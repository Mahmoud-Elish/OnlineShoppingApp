using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public record ItemDto
(
    int Id,string? ItemName, string? Description, string UOM, int Quantity, decimal Price
);
