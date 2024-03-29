﻿using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using Microsoft.Extensions.Options;

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
            {
                throw new Exception("DateStartingFetchingRates hasnt been set in appsettings");
            }

        }

        public Task<bool> FetchandSaveNewDataFromCurrencyRatesProvider()
        {
            _logger.LogInformation("Fetching data from Currency repository");
            DateTime CurrencyAPILastPublicatonDate = _CurrencyRatesRepository.GetDateLastPublication();
            DateTime WarehouseLastSuccesfulllFetchedPublishedDate = _CurrencyPowerWarehouseRepository.LastSuccessfullFetchedPublishedDate();
            if (CurrencyAPILastPublicatonDate > WarehouseLastSuccesfulllFetchedPublishedDate)
            {
                _logger.LogInformation($"Found new currency data from {CurrencyAPILastPublicatonDate}: Rates  in database are from {WarehouseLastSuccesfulllFetchedPublishedDate}");
                try
                {
                    IEnumerable<Currency> Currencies = _CurrencyRatesRepository.GetAllCurrencies();
                    _CurrencyPowerWarehouseRepository.UpdateCurrencies(Currencies);
                    _logger.LogInformation($"Update currencies  in count {Currencies.Count()}");
                    Currencies = _CurrencyPowerWarehouseRepository.GetAllCurrencies();
                    Currencies.Where(c=>!c.BaseCurrency).ToList().ForEach(currency =>
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
                            decimal CurrentRate = -1;
                            do
                            {
                                RatesTo = RatesFrom.AddDays(_CurrencyRatesRepository.GetMaxRateRangeDays()-1);
                                if (RatesTo > CurrencyAPILastPublicatonDate)
                                    RatesTo = CurrencyAPILastPublicatonDate;
                                IList<CurrencyRate> CurrencyRates = _CurrencyRatesRepository.GetCurrencyRates(RatesFrom, RatesTo, currency).ToList();
                                if (CurrencyRates.Count > 0 || CurrentRate >-1)
                                {
                                    
                                    _CurrencyPowerWarehouseRepository.AddCurrencyRates(FillDateGaps(ref CurrentRate, currency,CurrencyRates, RatesFrom, RatesTo));
                                }
                                RatesFrom = RatesFrom.AddDays(_CurrencyRatesRepository.GetMaxRateRangeDays());

                            } while (RatesFrom < CurrencyAPILastPublicatonDate);
                        }
                        else
                            _logger.LogInformation($"FCurrency {currency.Code} is up to date. Last update : {CurrencyLastUpdate}");

                    });
                    _CurrencyPowerWarehouseRepository.SetSuccessfullFetch(CurrencyAPILastPublicatonDate);
                }catch (Exception e)
                {
                    _CurrencyPowerWarehouseRepository.SetFailedFetch(e.Message+ " " +e.StackTrace.ToString());
                    return Task.FromResult(false);
                }

            }
            else
                _logger.LogInformation($"Rates in API are from {CurrencyAPILastPublicatonDate}: Rates  in database are from {WarehouseLastSuccesfulllFetchedPublishedDate} : No need to download reates");
            return Task.FromResult(true);

        }



        public IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode, string ChoosenReferenceCurrencies)
        {
            return _CurrencyPowerWarehouseRepository.GetCurrencyPowerRange(DateFrom, DateTo, CurrencyCode, ChoosenReferenceCurrencies);
        }

        public IEnumerable<CurrencyPowerChange> GetCurrencyPowerRange(DateTime DateFrom, DateTime DateTo, string CurrencyCode)
        {
            string RefCurrenciesStr = string.Join(",", _CurrencyPowerWarehouseRepository.GetReferenceCurrencies().Where(c => c.Code != CurrencyCode).Select(c => c.Code));
            return _CurrencyPowerWarehouseRepository.GetCurrencyPowerRange(DateFrom, DateTo, CurrencyCode, RefCurrenciesStr);
        }



        private IList<CurrencyRate> FillDateGaps(ref decimal pCurrentRate, Currency currency, IList<CurrencyRate> currencyRates, DateTime ratesFrom, DateTime ratesTo)
        {
            DateTime CurrentDate = ratesFrom;
            IList<CurrencyRate> currencyRatesOrdered = currencyRates.OrderBy(o =>  o.DateOfRate).ToList();
            decimal CurrentRate = pCurrentRate;
            decimal LastIndex = currencyRatesOrdered.Count() - 1;
            int itemIndex = 0;
            do
            {
                if (itemIndex > LastIndex)
                    currencyRatesOrdered.Add(new CurrencyRate() { DateOfRate = CurrentDate, Currency = currency, RateToBaseCurrency = CurrentRate });
                else
                {
                    if (currencyRatesOrdered[itemIndex].DateOfRate > CurrentDate)
                    {
                        if(CurrentRate>=0)
                            currencyRatesOrdered.Add(new CurrencyRate() { DateOfRate = CurrentDate, Currency = currency, RateToBaseCurrency = CurrentRate });
                    }
                    else
                    {
                        CurrentRate = currencyRatesOrdered[itemIndex].RateToBaseCurrency;
                        itemIndex++;
                    }
                }
                CurrentDate = CurrentDate.AddDays(1);
             
            }
            while (CurrentDate <= ratesTo);
            pCurrentRate = CurrentRate;
            return currencyRatesOrdered.OrderBy(o => o.DateOfRate).ToList();
        }

        IEnumerable<Currency> ICurrencyProcessingService.GetAllCurrencies()
        {
            return _CurrencyPowerWarehouseRepository.GetAllCurrencies();
        }

        IEnumerable<Currency> ICurrencyProcessingService.GetReferenceCurrencies()
        {
            return _CurrencyPowerWarehouseRepository.GetReferenceCurrencies();
        }
    }
}
