using CurrencyService.Model;
using CurrencyService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUnitTestCurrencyServiceProject.MoqClasses
{
    class CurrencyAPIMoq : ICurrencyRatesRepository
    {
        public CurrencyAPIMoq()
        {
            FetchedCurrencies = new List<Currency>();
            FetchedRates = new List<CurrencyRate>();
    }

        public List<Currency> FetchedCurrencies { get; set; }
        public List<CurrencyRate> FetchedRates { get; set; }
        public DateTime LastPublication { get; set; }
        public IEnumerable<Currency> GetAllCurrencies()
        {
            List<Currency> allcurencies = FetchedCurrencies.Where(i => true).ToList();
            allcurencies.Add(this.GetBaseCurrency());
            return allcurencies;
        }

        public Currency GetBaseCurrency()
        {
            return new Currency() { Code = "PLN", Desription = "polski złoty", BaseCurrency = true, ReferenceCurrency = true };


        }

        public IEnumerable<CurrencyRate> GetCurrencyRates(DateTime DateFrom, DateTime DateTo, Currency currency)
        {
            if (DateFrom.AddDays(GetMaxRateRangeDays() - 1) < DateTo)
                throw new Exception($"To wide date range {DateFrom} to {DateTo}");
            return FetchedRates.FindAll(r => r.Currency.Code == currency.Code && r.DateOfRate >= DateFrom && r.DateOfRate <= DateTo);


        }

        public DateTime GetDateLastPublication()
        {
            return LastPublication;
        }

        public DateTime GetDateLastPublication(Currency currency)
        {
            List<CurrencyRate> Rates = FetchedRates.FindAll(r => r.Currency.Code == currency.Code);
            Rates.Sort((x, y) => x.DateOfRate.CompareTo(y.DateOfRate));
            return Rates.First().DateOfRate;
        }

        public int GetMaxRateRangeDays()
        {
            return 365;
        }

        public void ClearAllData()
        {
            FetchedCurrencies.Clear();
            FetchedRates.Clear();
        }
        public void  AddCurrency(Currency currency)
        { FetchedCurrencies.Add(currency); }
        public void AddRate(CurrencyRate rate)
        {
            FetchedRates.Add(rate);
        }
    }
}
