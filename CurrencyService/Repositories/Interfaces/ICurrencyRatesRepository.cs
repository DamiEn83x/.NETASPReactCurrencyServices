using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public interface ICurrencyRatesRepository
    {
        IEnumerable<CurrencyRate> GetCurrencyRates(DateTime DateFrom, DateTime DateTo, Currency currency);
        IEnumerable<Currency> GetAllCurrencies();
        Currency GetBaseCurrency();
        DateTime GetDateLastPublication();
        DateTime GetDateLastPublication(Currency currency);
    }
}