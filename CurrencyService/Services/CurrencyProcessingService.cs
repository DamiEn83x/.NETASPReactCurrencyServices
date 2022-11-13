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

        }

        public async Task<bool> FetchandSaveNewDataFromCurrencyRatesProvider()
        {
            Console.WriteLine("Fetching data from Currency repository");
            _logger.LogInformation("Fetching data from Currency repository");
            DateTime CurrencyAPILastPublicatonDate = _CurrencyRatesRepository.GetDateLastPublication();
            DateTime WarehouseLastSuccesfulllUpdateDate = _CurrencyPowerWarehouseRepository.LastSuccessfulllUpdateDate();
            if (CurrencyAPILastPublicatonDate > WarehouseLastSuccesfulllUpdateDate)
            {
                IEnumerable<Currency> Currencies =_CurrencyRatesRepository.GetAllCurrencies();
                _CurrencyPowerWarehouseRepository.UpdateCurrencies(Currencies);
                Currencies.ToList().ForEach(currency => {
                    DateTime CurrencyLastUpdate = _CurrencyPowerWarehouseRepository.LastCurrencyRateDate(currency);
                    if (CurrencyLastUpdate==DateTime.MinValue)
                    {
                        CurrencyLastUpdate = _DateStartingFetchingRates.AddDays(-1);
                    }
                    DateTime RatesFrom=CurrencyLastUpdate;
                    DateTime RatesTo= DateTime.MinValue;
                    do
                    {
                        RatesTo = new DateTime(RatesFrom.Year, 12, 31);
                        if (RatesTo > CurrencyAPILastPublicatonDate)
                            RatesTo = CurrencyAPILastPublicatonDate;
                        IList<CurrencyRate> CurrencyRates = _CurrencyRatesRepository.GetCurrencyRates(RatesFrom, RatesTo, currency).ToList();
                        if (CurrencyRates.Count > 0)
                        {
                            _CurrencyPowerWarehouseRepository.AddCurrencyRates(CurrencyRates);
                        }
                        RatesFrom = new DateTime(RatesFrom.Year + 1, 1, 1);

                    } while (RatesFrom < CurrencyAPILastPublicatonDate);

                });

            }
            return true;

        }
    }
}
