using CurrencyService.Data;
using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly DbContext _ctx;
        private readonly DbSet<CurrencyRate> _currencyRates;
        private readonly DbSet<Currency> _Currencies;
        private readonly DbSet<CurrencyPowerChange> _CurrencyPawerChanges;
        private readonly DbSet<RatesDownload>  _RatesDownloads;
        private readonly DbSet<CurrencyRate> _CurrencyRates;

        public CurrencyPowerWarehouseSQLRepository(ILogger<CurrencyPowerWarehouseSQLRepository> logger, DbContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
            _currencyRates = _ctx.Set<CurrencyRate>();
            _CurrencyPawerChanges = _ctx.Set<CurrencyPowerChange>();
            _Currencies = _ctx.Set<Currency>();
            _RatesDownloads = _ctx.Set<RatesDownload>();
            _CurrencyRates =  _ctx.Set<CurrencyRate>();
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
            _currencyRates.AddRange(currenciesRates);
            _ctx.SaveChanges();
        }

        public bool CurrencyExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        public bool DataBaseIsEmpty()
        {
            return !(_ctx.Database.SqlQuery<int?>($@"SELECT 1 as Value FROM sys.tables AS T
                         INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id
                         WHERE S.Name = 'dbo' AND T.Name = '__EFMigrationsHistory'").SingleOrDefault() != null);
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            throw new NotImplementedException();
        }

        public Currency GetCurrencyByCode(string code)
        {
            return _Currencies.Where(c => c.Code == code).FirstOrDefault();
        }

        public IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode, string ChoosenReferenceCurrencies)
        {
            return _CurrencyPawerChanges.FromSqlRaw($"dbo.SelectCurrencyPowers {DateFrom},{DateTo},{CurrencyCode},{ChoosenReferenceCurrencies}")
                      .ToList();
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
            _RatesDownloads.Add(FetchStatus);
            _ctx.SaveChanges();
;        }

        public void SetSuccessfullFetch(DateTime PublicationDate)
        {
            RatesDownload FetchStatus = new RatesDownload() { FetchDate = DateTime.Now, Successfull = true ,PublishedDate=PublicationDate};
            _RatesDownloads.Add(FetchStatus);
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
            return _Currencies.ToList();
        }


        IEnumerable<Currency> ICurrencyPowerWarehouseRepository.GetReferenceCurrencies()
        {
            return _Currencies.Where(c => c.ReferenceCurrency).ToList();
        }


        DateTime ICurrencyPowerWarehouseRepository.LastCurrencyRateDate(Currency currency)
        {
            CurrencyRate lastdownload = _CurrencyRates.Where(s => s.Currency.Code==currency.Code).OrderByDescending(o => o.DateOfRate).FirstOrDefault();
            if (lastdownload != null)
                return lastdownload.DateOfRate;
            else
                return DateTime.MinValue;
        }

        DateTime ICurrencyPowerWarehouseRepository.LastSuccessfullFetchedPublishedDate()
        {
            RatesDownload lastdownload = _RatesDownloads.Where(s => s.Successfull).OrderByDescending(o => o.PublishedDate).FirstOrDefault();
            if (lastdownload != null)
                return lastdownload.PublishedDate;
            else
                return DateTime.MinValue;
        }

        void ICurrencyPowerWarehouseRepository.UpdateCurrencies(IEnumerable<Currency> currencies)
        {

            currencies.ToList().ForEach(currency =>
            {
                Currency stored = _Currencies.SingleOrDefault(b => b.Code == currency.Code);
                if (stored != null)
                {
                    if (stored.Desription != currency.Desription ||
                        stored.BaseCurrency != currency.BaseCurrency ||
                        stored.ReferenceCurrency != currency.ReferenceCurrency)
                    {
                        stored.Desription = currency.Desription;
                        stored.BaseCurrency = currency.BaseCurrency;
                        stored.ReferenceCurrency = currency.ReferenceCurrency;
                        _ctx.SaveChanges();
                    }
                }
                else
                {
                    _Currencies.Add(currency);
                    _ctx.SaveChanges();
                }

             });
        }
    }
}
