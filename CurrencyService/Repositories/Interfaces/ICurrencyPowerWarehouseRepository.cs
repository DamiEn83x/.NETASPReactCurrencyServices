using CurrencyService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories.Inrfaces
{

    public interface ICurrencyPowerWarehouseRepository
    {
        IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, Currency Currency, IEnumerable<Currency> ChoosenReferenceCurrencies);
        IEnumerable<Currency> GetReferenceCurrencies();
        IEnumerable<Currency> GetAllCurrencies();

        bool CurrencyExists(Currency currency);
        int AddCurrencyIfNotExists(Currency currency);

        int AddCurrencyRate(Currency currency, DateTime date, decimal rate);

        DateTime LastAnyCurrencyRateDate();

        DateTime LastCurrencyRateDate(Currency currency);

        DateTime LastSuccessfulllUpdateDate();

        void UpdateCurrencies(IEnumerable<Currency> currencies);

        void AddCurrencyRates(IEnumerable<CurrencyRate> currenciesRates);



    }
}
