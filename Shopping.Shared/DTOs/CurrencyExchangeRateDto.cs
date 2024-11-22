using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public class CurrencyExchangeRateDto
{
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}
