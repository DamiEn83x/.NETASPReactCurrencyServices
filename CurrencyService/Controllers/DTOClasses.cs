using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Controllers
{
    public class CurrencyDTO
    {
        public string Code { get; set; }
        public string Desription { get; set; }
        public bool ReferenceCurrency { get; set; }
        public bool BaseCurrency { get; set; }

        public static CurrencyDTO FromCurrency(Currency currency)
        {
            return new CurrencyDTO() { Code = currency.Code, Desription = currency.Desription, ReferenceCurrency = currency.ReferenceCurrency, BaseCurrency = currency.BaseCurrency }; 
        }
    }

    public class CurrencyPowerChangeDTO
    {
        public DateTime Date { get; set; }
        public decimal PowerIndicator { get; set; }
        public static CurrencyPowerChangeDTO CurrencyPowerChange(CurrencyPowerChange currencypowers)
        {
            return new CurrencyPowerChangeDTO() { Date = currencypowers.Date, PowerIndicator = currencypowers.PowerIndicator };
        }
    }
}
