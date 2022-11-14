using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Services
{

    public class CurrencyProcessingServiceOptions
    {
        public const string Position = "CurrencyProcessing";
        public DateTime DateStartingFetchingRates { get; set; }
    }
    public class CurrencyProcessingService : ICurrencyProcessingService
    {
        private ICurrencyRatesRepository _CurrencyRatesRepository;
        private ICurrencyPowerWarehouseRepository _CurrencyPowerWarehouseRepository;
        private DateTime _DateStartingFetchingRates;
        ILogger<CurrencyProcessingService> _logger;
        public CurrencyProcessingService(ILogger<CurrencyProcessingService>  logger,ICurrencyRatesRepository CurrencyRatesRepository , ICurrencyPowerWarehouseRepository CurrencyPowerWarehouseRepository, IOptions<CurrencyProcessingServiceOptions> options)
        {
            _logger = logger;
            _CurrencyRatesRepository = CurrencyRatesRepository;
            _CurrencyPowerWarehouseRepository = CurrencyPowerWarehouseRepository;
            _DateStartingFetchingRates = ((CurrencyProcessingServiceOptions)options.Value).DateStartingFetchingRates;
            if (_DateStartingFetchingRates == DateTime.MinValue)
                throw new Exception("DateStartingFetchingRates hasnt been set in appsettings");

        }

        public async Task<bool> FetchandSaveNewDataFromCurrencyRatesProvider()
        {
            Console.WriteLine("Fetching data from Currency repository");
            _logger.LogInformation("Fetching data from Currency repository");
            DateTime CurrencyAPILastPublicatonDate = _CurrencyRatesRepository.GetDateLastPublication();
            DateTime WarehouseLastSuccesfulllUpdateDate = _CurrencyPowerWarehouseRepository.LastSuccessfullUpdateDate();
            if (CurrencyAPILastPublicatonDate > WarehouseLastSuccesfulllUpdateDate)
            {
                _logger.LogInformation($"Found new currency data from {CurrencyAPILastPublicatonDate}: Rates  in database are from {WarehouseLastSuccesfulllUpdateDate}");
                try
                {
                    IEnumerable<Currency> Currencies = _CurrencyRatesRepository.GetAllCurrencies();
                    _CurrencyPowerWarehouseRepository.UpdateCurrencies(Currencies);
                    _logger.LogInformation($"Update currencies  in count {Currencies.Count()}");
                    Currencies = _CurrencyPowerWarehouseRepository.GetAllCurrencies();
                    Currencies.ToList().ForEach(currency =>
                    {
                        DateTime CurrencyLastUpdate = _CurrencyPowerWarehouseRepository.LastCurrencyRateDate(currency);
                        if (CurrencyLastUpdate < CurrencyAPILastPublicatonDate)
                        {
                            _logger.LogInformation($"Fetching rates for  {currency.Code} . Lates  db  rate is from {CurrencyLastUpdate}");
                            if (CurrencyLastUpdate == DateTime.MinValue)
                            {
                                CurrencyLastUpdate = _DateStartingFetchingRates.AddDays(-1);
                            }
                            DateTime RatesFrom = CurrencyLastUpdate;
                            DateTime RatesTo = DateTime.MinValue;
                            do
                            {
                                RatesTo = new DateTime(RatesFrom.Year, 12, 31);
                                if (RatesTo > CurrencyAPILastPublicatonDate)
                                    RatesTo = CurrencyAPILastPublicatonDate;
                                IList<CurrencyRate> CurrencyRates = _CurrencyRatesRepository.GetCurrencyRates(RatesFrom, RatesTo, currency).ToList();
                                if (CurrencyRates.Count > 0)
                                {
                                    
                                    _CurrencyPowerWarehouseRepository.AddCurrencyRates(FillDateGaps(CurrencyRates, RatesFrom, RatesTo));
                                }
                                RatesFrom = new DateTime(RatesFrom.Year + 1, 1, 1);

                            } while (RatesFrom < CurrencyAPILastPublicatonDate);
                        }

                    });
                    _CurrencyPowerWarehouseRepository.SetSuccessfullFetch();
                }catch (Exception e)
                {
                    _CurrencyPowerWarehouseRepository.SetFailedFetch(e.Message+ " " +e.StackTrace.ToString());
                    return false;
                }

            }
            return true;

        }

        private IList<CurrencyRate> FillDateGaps(IList<CurrencyRate> currencyRates, DateTime ratesFrom, DateTime ratesTo)
        {
            DateTime CurrentDate = ratesFrom;
            IList<CurrencyRate> currencyRatesOrdered = currencyRates.OrderBy(o =>  o.DateOfRate).ToList();
            CurrencyRate FirstRate = currencyRatesOrdered.First();
            decimal CurrentRate = currencyRatesOrdered.First().RateToBaseCurrency;
            decimal LastIndex = currencyRatesOrdered.Count() - 1;
            int itemIndex = 0;
            do
            {
                if (itemIndex > LastIndex)
                    currencyRatesOrdered.Add(new CurrencyRate() { DateOfRate = CurrentDate, Currency = FirstRate.Currency, RateToBaseCurrency = CurrentRate });
                else
                {
                    if (currencyRatesOrdered[itemIndex].DateOfRate > CurrentDate)
                    {
                        currencyRatesOrdered.Add(new CurrencyRate() { DateOfRate = CurrentDate, Currency = FirstRate.Currency, RateToBaseCurrency = CurrentRate });
                    }
                    else
                    {
                        itemIndex++;
                        CurrentRate = currencyRatesOrdered[itemIndex].RateToBaseCurrency;
                    }
                }
                CurrentDate = CurrentDate.AddDays(1);
             
            }
            while (CurrentDate <= ratesTo);

            return currencyRatesOrdered.OrderBy(o => o.DateOfRate).ToList();
        }
    }
}
