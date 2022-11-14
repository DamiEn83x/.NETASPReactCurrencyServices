using CurrencyService.Data;
using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories
{
    public class CurrencyPowerWarehouseSQLRepository : ICurrencyPowerWarehouseRepository
    {
        private readonly ILogger _logger;
        private readonly DataContext _ctx;
        public CurrencyPowerWarehouseSQLRepository(ILogger<CurrencyPowerWarehouseSQLRepository> logger, DataContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
        }

        public int AddCurrencyIfNotExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        public int AddCurrencyRate(Currency currency, DateTime date, decimal rate)
        {
            throw new NotImplementedException();
        }

        public void AddCurrencyRates(IEnumerable<CurrencyRate> currenciesRates)
        {
            _ctx.CurrencyRates.AddRange(currenciesRates);
            _ctx.SaveChanges();
        }

        public bool CurrencyExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, Currency Currency, IEnumerable<Currency> ChoosenReferenceCurrencies)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Currency> GetReferenceCurrencies()
        {
            throw new NotImplementedException();
        }

        public DateTime LastAnyCurrencyRateDate()
        {
            throw new NotImplementedException();
        }

        public DateTime LastCurrencyRateDate(Currency currency)
        {
            throw new NotImplementedException();
        }

        public void SetFailedFetch(string szError)
        {
            RatesDownload FetchStatus = new RatesDownload() { FetchDate = DateTime.Now, Error = szError, Successfull = false };
            _ctx.RatesDownloads.Add(FetchStatus);
            _ctx.SaveChanges();
;        }

        public void SetSuccessfullFetch()
        {
            RatesDownload FetchStatus = new RatesDownload() { FetchDate = DateTime.Now, Successfull = true };
            _ctx.RatesDownloads.Add(FetchStatus);
            _ctx.SaveChanges();
        }

        int ICurrencyPowerWarehouseRepository.AddCurrencyIfNotExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        int ICurrencyPowerWarehouseRepository.AddCurrencyRate(Currency currency, DateTime date, decimal rate)
        {
            throw new NotImplementedException();
        }

        bool ICurrencyPowerWarehouseRepository.CurrencyExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Currency> ICurrencyPowerWarehouseRepository.GetAllCurrencies()
        {
            throw new NotImplementedException();
        }

        IEnumerable<CurrencyPowerChange> ICurrencyPowerWarehouseRepository.GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, Currency Currency, IEnumerable<Currency> ChoosenReferenceCurrencies)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Currency> ICurrencyPowerWarehouseRepository.GetReferenceCurrencies()
        {
            throw new NotImplementedException();
        }


        DateTime ICurrencyPowerWarehouseRepository.LastCurrencyRateDate(Currency currency)
        {
            CurrencyRate lastdownload = _ctx.CurrencyRates.Where(s => s.Currency.Code==currency.Code).OrderByDescending(o => o.DateOfRate).FirstOrDefault();
            if (lastdownload != null)
                return lastdownload.DateOfRate;
            else
                return DateTime.MinValue;
        }

        DateTime ICurrencyPowerWarehouseRepository.LastSuccessfullUpdateDate()
        {
            RatesDownload lastdownload = _ctx.RatesDownloads.Where(s => s.Successfull).OrderByDescending(o => o.FetchDate).FirstOrDefault();
            if (lastdownload != null)
                return lastdownload.FetchDate;
            else
                return DateTime.MinValue;
        }

        void ICurrencyPowerWarehouseRepository.UpdateCurrencies(IEnumerable<Currency> currencies)
        {
            var missingRecords = currencies.Where(x => !_ctx.Currencies.Any(z => z.Code == x.Code)).ToList();
            if (missingRecords.Count > 0)
            {
                _ctx.Currencies.AddRange(missingRecords);
                _ctx.SaveChanges();
            }
        }
    }
}
