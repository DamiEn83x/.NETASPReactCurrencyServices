﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyService.Model;
using CurrencyService.Services;
using Microsoft.Extensions.Options;

namespace CurrencyService.Repositories.NBPAPI
{
    public class APINBPCurrencyRatesRepositoryOptions
    {
        public const string Position = "APINBPCurrencyRatesRepository";
        public string APIBaseURL { get; set; }
    }
  
    public class APINBPCurrencyRatesRepository : ICurrencyRatesRepository
    {
        const string APIDateFormat = "yyyy-MM-dd";
        private string _APIBaseURL ;
        public APINBPCurrencyRatesRepository(IOptions<APINBPCurrencyRatesRepositoryOptions> options)
        {
            _APIBaseURL = ((APINBPCurrencyRatesRepositoryOptions)options.Value).APIBaseURL;
        }

        public DateTime GetDateLastPublication()
        {
            DateTime Result=new DateTime();
            IEnumerable<APITableResultObject> CurrenciesTable = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_APIBaseURL);
                string Uri = "exchangerates/tables/A/?format=JSON";
                var responseTask = client.GetAsync(Uri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<APITableResultObject>>();
                    readTask.Wait();

                    CurrenciesTable = readTask.Result;
                    var cultureInfo = new CultureInfo("pl-PL");
                    Result = DateTime.Parse(CurrenciesTable.First().effectiveDate, cultureInfo);
                }
                else //web api sent error response 
                {
                    throw new  Exception($"Error durring fewtch data from  API:{client.BaseAddress+Uri}");
                }
            }
            return Result;
        }

        public DateTime GetDateLastPublication(Currency currency)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Currency> ICurrencyRatesRepository.GetAllCurrencies()
        {
            List<Currency> ResultCurrencies = new List<Currency>();
            IEnumerable<APITableResultObject> CurrenciesTable = null;
            for (int i = 0; i < 2; i++)
            {
                string table = "";
                if (i == 0)
                    table = "A";
                else
                    table = "B";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_APIBaseURL);
                    string Uri = $"exchangerates/tables/{table}/?format=JSON";
                    var responseTask = client.GetAsync(Uri);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<APITableResultObject>>();
                        readTask.Wait();

                        CurrenciesTable = readTask.Result;

                        CurrenciesTable.First().rates.ToList().ForEach(currency =>
                        {
                            ResultCurrencies.Add(new Currency() { Code = currency.code, Desription = currency.currency, Table = table });
                        });

                    }
                    else //web api sent error response 
                    {
                        throw new Exception($"Error durring fewtch data from  API:{client.BaseAddress + Uri}");
                    }
                }
            }

            return ResultCurrencies;
        }

        IEnumerable<CurrencyRate> ICurrencyRatesRepository.GetCurrencyRates(DateTime DateFrom, DateTime DateTo, Currency currency)
        {
            List<CurrencyRate> ResultCurrencyRates =  new List<CurrencyRate>();
            APIHeadRateResul CurrencyRates = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_APIBaseURL);
        
                string Uri = $"exchangerates/rates/{currency.Table}/{currency.Code}/{DateFrom.ToString(APIDateFormat)}/{DateTo.ToString(APIDateFormat)}/?format=JSON";
                var responseTask = client.GetAsync(Uri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<APIHeadRateResul>();
                    readTask.Wait();

                    CurrencyRates = readTask.Result;
                    var cultureInfo = new CultureInfo("pl-PL");
                    CurrencyRates.rates.ToList().ForEach(rate => {
                        ResultCurrencyRates.Add(new CurrencyRate() { Currency = currency, RateToBaseCurrency = rate.mid, DateOfRate = DateTime.Parse(rate.effectiveDate, cultureInfo) });

                    });
                }
                else //web api sent error response 
                {
                    throw new Exception($"Error durring fewtch data from  API:{client.BaseAddress + Uri}");
                }
            }
            return ResultCurrencyRates;
        }

        IEnumerable<Currency> ICurrencyRatesRepository.GetReferenceCurrencies()
        {
            throw new NotImplementedException();
        }
    }
}
