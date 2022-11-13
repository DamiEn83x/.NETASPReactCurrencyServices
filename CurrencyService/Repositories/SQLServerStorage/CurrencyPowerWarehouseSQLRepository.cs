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

        public CurrencyPowerWarehouseSQLRepository(ILogger<CurrencyPowerWarehouseSQLRepository> logger)
        {
            _logger = logger;
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
            currenciesRates.ToList().ForEach(rate => {
                _logger.LogInformation($"Adding new currrency rates for {rate.Currency.Code} rate {rate.RateToBaseCurrency}  date {rate.DateOfRate}  table{rate.Currency.Table} ");
            });

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

        public DateTime LastSuccessfulllUpdateDate()
        {
            return DateTime.Parse("2022-11-08", new CultureInfo("pl-PL"));
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

        DateTime ICurrencyPowerWarehouseRepository.LastAnyCurrencyRateDate()
        {
            throw new NotImplementedException();
        }

        DateTime ICurrencyPowerWarehouseRepository.LastCurrencyRateDate(Currency currency)
        {
            return DateTime.Parse("2022-11-06", new CultureInfo("pl-PL"));
        }

        DateTime ICurrencyPowerWarehouseRepository.LastSuccessfulllUpdateDate()
        {
            return DateTime.Parse("2022-11-06", new CultureInfo("pl-PL"));
        }

        void ICurrencyPowerWarehouseRepository.UpdateCurrencies(IEnumerable<Currency> currencies)
        {
            Console.WriteLine("Updating Currencies list");
        }
    }
}
