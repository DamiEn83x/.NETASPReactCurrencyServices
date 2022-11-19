using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using CurrencyService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using XUnitTestCurrencyServiceProject.MoqClasses;


namespace XUnitTestCurrencyServiceProject
{
    public class UnitTest1
    {
        ICurrencyProcessingService _ICurrencyProcessingService;
        WareHouseMock _CurrencyPowerWarehouseRepository;
        CurrencyAPIMoq _ICurrencyRatesRepository;

        public UnitTest1()
        {
            ILogger<CurrencyProcessingService> ILoggerService = new NullLogger<CurrencyProcessingService>();
            _CurrencyPowerWarehouseRepository = new WareHouseMock();
            _ICurrencyRatesRepository = new CurrencyAPIMoq();

            var options  = Options.Create(new CurrencyProcessingServiceOptions() { DateStartingFetchingRates = DateTime.Parse("2020-01-01", new CultureInfo("pl-PL")) });
            _ICurrencyProcessingService = new CurrencyProcessingService(ILoggerService, _ICurrencyRatesRepository, _CurrencyPowerWarehouseRepository, options);
        }

        [Fact]
        public void Test1()
        {
            _ICurrencyRatesRepository.ClearAllData();
            _CurrencyPowerWarehouseRepository.ClearAllData();
            string fetched = "&USD,REF,D2022-10-01,r0.25,D2022-10-10,r0.25,&EUR,REF,D2022-10-01,r0.25,D2022-10-10,r0.25";
            List<string> fetchedList= fetched.Split(',').ToList();
            Currency currency = null;
            CurrencyRate rate = null;
            fetchedList.ForEach(i => {
                if (i.StartsWith("&"))
                {
                    currency = new Currency() { Code = i.Substring(1) };
                    _ICurrencyRatesRepository.AddCurrency(currency);
                }
                else if (i == "REF")
                {
                    currency.ReferenceCurrency = true;
                }
                if (i.StartsWith("D"))
                {
                    rate = new CurrencyRate() { DateOfRate = DateTime.Parse(i.Substring(1), new CultureInfo("pl-PL")),Currency=currency};
                    _ICurrencyRatesRepository.AddRate(rate);
                }
                if (i.StartsWith("r"))
                {
                    rate.RateToBaseCurrency= decimal.Parse(i.Substring(1));
                }



            });
           
            _ICurrencyRatesRepository.LastPublication = DateTime.Parse("2022-11-10", new CultureInfo("pl-PL"));

            _ICurrencyProcessingService.FetchandSaveNewDataFromCurrencyRatesProvider();
            string expected = "EUR,2022-10-01,0.25,2022-10-10,0.25,'PLN,USD,2022-10-01,0.25,2022-10-10,0.25";
            Assert.Equal(expected, _CurrencyPowerWarehouseRepository.GetDatabaseStateCode());

        }
    }
}
