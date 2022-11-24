using CurrencyService.Model;
using CurrencyService.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
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

            var options  = Options.Create(new CurrencyProcessingServiceOptions() { DateStartingFetchingRates = DateTime.Parse("2000-01-01", new CultureInfo("pl-PL")) });
            _ICurrencyProcessingService = new CurrencyProcessingService(ILoggerService, _ICurrencyRatesRepository, _CurrencyPowerWarehouseRepository, options);
        }

        [Theory]
        [InlineData("LP2022-11-11,&COP,D2018-01-01,r0.10,D2018-01-02,r0.123,D2022-12-12,r0.11,&USD,REF,D2020-10-01,r0.21,D2022-10-10,r0.25,&EUR,REF,D2021-10-01,r0.25,D2022-10-10,r0.15", "COP,1776,2018-01-01,0.10,2022-11-11,0.123,EUR,407,2021-10-01,0.25,2022-11-11,0.15,PLN,USD,772,2020-10-01,0.21,2022-11-11,0.25,")]
        [InlineData("LP2022-10-11,&USD,REF,D2022-10-01,r0.25,D2022-10-10,r4.25,&EUR,REF,D2022-10-01,r4.25,D2022-10-10,r4.25", "EUR,11,2022-10-01,4.25,2022-10-11,4.25,PLN,USD,11,2022-10-01,0.25,2022-10-11,4.25,")]
        [InlineData("LP2022-11-11,&USD,REF,D2020-10-01,r0.21,D2022-10-10,r4.25,&EUR,REF,D2021-10-01,r4.25,D2022-10-10,r4.15", "EUR,407,2021-10-01,4.25,2022-11-11,4.15,PLN,USD,772,2020-10-01,0.21,2022-11-11,4.25,")]
        public async void Test1(string fetched,string expected)
        {
            _ICurrencyRatesRepository.ClearAllData();
            _CurrencyPowerWarehouseRepository.ClearAllData();
      
       
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
                else if (i.StartsWith("D"))
                {
                    rate = new CurrencyRate() { DateOfRate = DateTime.Parse(i.Substring(1), new CultureInfo("pl-PL")),Currency=currency};
                    _ICurrencyRatesRepository.AddRate(rate);
                }
                else if (i.StartsWith("r"))
                {
                    rate.RateToBaseCurrency= decimal.Parse(i.Substring(1));
                }
                else if (i.StartsWith("LP"))
                {
                    _ICurrencyRatesRepository.LastPublication = DateTime.Parse(i.Substring(2), new CultureInfo("pl-PL"));
                }            

            });
           
         

            bool result  =  await _ICurrencyProcessingService.FetchandSaveNewDataFromCurrencyRatesProvider();
            Assert.Equal(true, result);
            Assert.Equal(expected, _CurrencyPowerWarehouseRepository.GetDatabaseStateCode());

        }
    }
}
