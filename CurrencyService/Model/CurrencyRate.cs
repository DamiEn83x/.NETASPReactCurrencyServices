using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class CurrencyRate
    {
        public int CurrencyRateId { get; set; }
        public Currency Currency { get; set; }
        public decimal RateToBaseCurrency{ get; set; }
        public DateTime DateOfRate { get; set; }
    }
}
