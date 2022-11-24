using CurrencyService.Model;

namespace CurrencyService.Repositories.Inrfaces
{

    public interface ICurrencyPowerWarehouseRepository
    {
        IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string  CurrencyCode, string ChoosenReferenceCurrencies);
        IEnumerable<Currency> GetReferenceCurrencies();
        IEnumerable<Currency> GetAllCurrencies();

        Currency GetCurrencyByCode(string code);

        bool CurrencyExists(Currency currency);
        int AddCurrencyIfNotExists(Currency currency);

        int AddCurrencyRate(Currency currency, DateTime date, decimal rate);

        DateTime LastCurrencyRateDate(Currency currency);

        DateTime LastSuccessfullFetchedPublishedDate();

        void SetSuccessfullFetch(DateTime PublicationDate);
        void SetFailedFetch(string Error);

        void UpdateCurrencies(IEnumerable<Currency> currencies);

        void AddCurrencyRates(IEnumerable<CurrencyRate> currenciesRates);



    }
}
