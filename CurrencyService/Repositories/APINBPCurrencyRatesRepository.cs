using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.Model;

namespace CurrencyService.Services
{
    public class APINBPCurrencyRatesRepository : ICurrencyRatesRepository
    {

        IEnumerable<Currency> ICurrencyRatesRepository.GetAllCurrencies()
        {
            throw new NotImplementedException();
        }

        IEnumerable<decimal> ICurrencyRatesRepository.GetCurrencyRates(DateTime DateFrom, DateTime DateTo, string currency)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Currency> ICurrencyRatesRepository.GetReferenceCurrencies()
        {
            throw new NotImplementedException();
        }
    }
}
