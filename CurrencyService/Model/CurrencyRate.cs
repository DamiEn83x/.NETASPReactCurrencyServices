using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class CurrencyRate
    {
        public Currency Currency;
        public decimal RateToBaseCurrency{ get; set; }
        public DateTime DateOfRate { get; set; }
    }
}
