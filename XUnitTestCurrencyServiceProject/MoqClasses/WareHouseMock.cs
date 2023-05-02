using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTestCurrencyServiceProject.MoqClasses
{
    class WareHouseMock : ICurrencyPowerWarehouseRepository
    {
        List<Currency> _currencies;
        List<CurrencyRate> _rates;
        DateTime _SuccesfullFetch;
        DateTime _SuccesfullFetchPublication;

        public WareHouseMock()
        {
            _currencies = new List<Currency>();
            _rates = new List<CurrencyRate>();
            _SuccesfullFetch = DateTime.MinValue;
            _SuccesfullFetchPublication  = DateTime.MinValue;
        }

        public int AddCurrencyIfNotExists(Currency currency)
        {
            if (_currencies.Find(i => i.Code == currency.Code)==null)
            _currencies.Add(currency);
            return 0;
        }

        public int AddCurrencyRate(Currency currency, DateTime date, decimal rate)
        {
            _rates.Add(new CurrencyRate(){ Currency = currency, DateOfRate = date, RateToBaseCurrency = rate });
            return 0;
        }

        public void AddCurrencyRates(IEnumerable<CurrencyRate> currenciesRates)
        {
            _rates.AddRange(currenciesRates);
        }

        public bool CurrencyExists(Currency currency)
        {
            return (_currencies.Find(i => i.Code == currency.Code) != null);
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            return _currencies;
        }

        public Currency GetCurrencyByCode(string code)
        {
            return _currencies.Find(i => i.Code == code);
        }

        public IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode, string ChoosenReferenceCurrencies)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Currency> GetReferenceCurrencies()
        {
             return _currencies.FindAll(c => c.ReferenceCurrency);
        }

        public DateTime LastCurrencyRateDate(Currency currency)
        {
            DateTime lastRate = DateTime.MinValue;

            _rates.FindAll(c => c.Currency.Code == currency.Code).ForEach(c =>
            {
                if (c.DateOfRate > lastRate)
                    lastRate = c.DateOfRate;
            });
            return lastRate;
        }

        public DateTime LastSuccessfullUpdateDate()
        {
            return _SuccesfullFetch;
        }

        public void SetFailedFetch(string Error)
        {
            //do nothing
        }

        public void SetSuccessfullFetch(DateTime PublicationDate)
        {
            _SuccesfullFetch = DateTime.Now;
            _SuccesfullFetchPublication = PublicationDate;
        }

        public void UpdateCurrencies(IEnumerable<Currency> currencies)
        {
            currencies.ToList().ForEach(currency =>
            {
                Currency stored = _currencies.Find(b => b.Code == currency.Code);
                if (stored != null)
                {
                    if (stored.Desription != currency.Desription ||
                        stored.BaseCurrency != currency.BaseCurrency ||
                        stored.ReferenceCurrency != currency.ReferenceCurrency)
                    {
                        stored.Desription = currency.Desription;
                        stored.BaseCurrency = currency.BaseCurrency;
                        stored.ReferenceCurrency = currency.ReferenceCurrency;

                    }
                }
                else
                {
                    _currencies.Add(currency);
                }

            });
        }

        public void ClearAllData()
        {
            _currencies.Clear();
            _rates.Clear();
        }

        public string GetDatabaseStateCode()
        {
            string output = "";

            _currencies.OrderBy(i => i.Code).ToList().ForEach(Currency=> { output = output + Currency.Code+',';
                if (!Currency.BaseCurrency)
                {
                    List<CurrencyRate> rates = _rates.Where(i => i.Currency.Code == Currency.Code).OrderBy(i => i.DateOfRate).ToList();
                    if(rates.Count()>0)
                        output = output + rates.Count + ',' + rates.First().DateOfRate.ToString("yyyy-MM-dd") + ','+rates.First().RateToBaseCurrency.ToString() + ',' + rates.Last().DateOfRate.ToString("yyyy-MM-dd") + ','+rates.Last().RateToBaseCurrency.ToString() + ',';
                }

            });
            return output;

        }

        public DateTime LastSuccessfullFetchedPublishedDate()
        {
            return this._SuccesfullFetchPublication;
        }

        public bool DataBaseIsEmpty()
        {
            throw new NotImplementedException();
        }

        public void DoSomeTuttorialTests()
        {
            throw new NotImplementedException();
        }
    }
}
