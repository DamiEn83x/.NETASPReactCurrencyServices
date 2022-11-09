using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public interface ICurrencyRatesRepository
    {
        IEnumerable<decimal> GetCurrencyRates(DateTime DateFrom, DateTime DateTo, string currency);
        IEnumerable<Currency> GetReferenceCurrencies();
        IEnumerable<Currency> GetAllCurrencies();
    }
}