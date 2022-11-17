using CurrencyService.Controllers;
using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public interface ICurrencyProcessingService
    {
        Task<bool> FetchandSaveNewDataFromCurrencyRatesProvider();

        IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode, string ChoosenReferenceCurrencies);
        IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode);
        IEnumerable<Currency> GetReferenceCurrencies();
        IEnumerable<Currency> GetAllCurrencies();
    }
}