﻿using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUnitTestCurrencyServiceProject.MoqClasses
{
    class WareHouseMock : ICurrencyPowerWarehouseRepository
    {
        List<Currency> _currencies;
        List<CurrencyRate> _rates;
        DateTime _SuccesfullFetch;

        public WareHouseMock()
        {
            _currencies = new List<Currency>();
            _rates = new List<CurrencyRate>();
            _SuccesfullFetch = DateTime.MinValue;
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
            throw new NotImplementedException();
        }

        public void SetSuccessfullFetch()
        {
            _SuccesfullFetch = DateTime.Now;
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
                _rates.Where(i => i.Currency.Code == Currency.Code).OrderBy(i => i.DateOfRate).ToList().ForEach(rate => { output = output + rate.DateOfRate.ToString("yyyy-MM-dd") + "," + rate.RateToBaseCurrency.ToString() + ","; });
            });
            return output;

        }
    }
}
