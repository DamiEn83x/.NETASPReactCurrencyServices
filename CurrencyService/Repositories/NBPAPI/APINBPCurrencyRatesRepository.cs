using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CurrencyService.Model;
using CurrencyService.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

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
        private Dictionary<string, string> _CodeTableMap = null;
        public APINBPCurrencyRatesRepository(IOptions<APINBPCurrencyRatesRepositoryOptions> options)
        {
   
            _APIBaseURL = ((APINBPCurrencyRatesRepositoryOptions)options.Value).APIBaseURL;
        }

        public Currency GetBaseCurrency()
        {
            return new Currency() { Code = "PLN", Desription = "polski złoty", BaseCurrency = true,ReferenceCurrency=true };
        }

        public DateTime GetDateLastPublication()
        {
            DateTime Result=new DateTime();
            IEnumerable<APITableResultObject> CurrenciesTable = null;
            for (int i = 0; i < 3; i++)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_APIBaseURL);
                    string Uri = "exchangerates/tables/A/?format=JSON";
                    var responseTask = client.GetAsync(Uri);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        string jsonContent = result.Content.ReadAsStringAsync().Result;
                        CurrenciesTable = JsonConvert.DeserializeObject<IList<APITableResultObject>>(jsonContent); 

                        var cultureInfo = new CultureInfo("pl-PL");
                        Result = DateTime.Parse(CurrenciesTable.First().effectiveDate, cultureInfo);
                        break;
                    }
                    else //web api sent error response 
                    {   if(i>=2)
                            throw new Exception($"Error durring fewtch data from  API:{client.BaseAddress + Uri}");
                    }
                }
            }
            return Result;
        }

        public DateTime GetDateLastPublication(Currency currency)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            Dictionary<string, string>  CodeTableMap=new Dictionary<string, string>();
            List<Currency> ResultCurrencies = new List<Currency>();
            IEnumerable<APITableResultObject> CurrenciesTable = null;
            for (int i = 0; i < 2; i++)
            {
                string table = "";
                if (i == 0)
                    table = "A";
                else
                    table = "B";
                for (int j = 0; j < 3; j++)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(_APIBaseURL);
                        string Uri = $"exchangerates/tables/{table}/?format=JSON";
                        var responseTask = client.GetAsync(Uri);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            string jsonContent = result.Content.ReadAsStringAsync().Result;

                            CurrenciesTable = JsonConvert.DeserializeObject<IList<APITableResultObject>>(jsonContent);

                            CurrenciesTable.First().rates.ToList().ForEach(currency =>
                            {
                                CodeTableMap.Add(currency.code, table);
                                ResultCurrencies.Add(new Currency() { Code = currency.code, Desription = currency.currency, ReferenceCurrency = (table=="A"), BaseCurrency=false });
                            });
                            break;

                        }
                        else //web api sent error response 
                        {
                            if(j>=2)
                                throw new Exception($"Error durring fewtch data from  API:{client.BaseAddress + Uri}");
                        }
                    }
                }
            }

            ResultCurrencies.Add(GetBaseCurrency());
            this._CodeTableMap = CodeTableMap;
            return ResultCurrencies;
        }

        IEnumerable<CurrencyRate> ICurrencyRatesRepository.GetCurrencyRates(DateTime DateFrom, DateTime DateTo, Currency currency)
        {
            if (_CodeTableMap == null)
            {
                this.GetAllCurrencies();
            }
            List<CurrencyRate> ResultCurrencyRates =  new List<CurrencyRate>();
            APIHeadRateResul CurrencyRates = null;
            for (int i = 0; i < 3; i++)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_APIBaseURL);

                    string Uri = $"exchangerates/rates/{_CodeTableMap[currency.Code]}/{currency.Code}/{DateFrom.ToString(APIDateFormat)}/{DateTo.ToString(APIDateFormat)}/?format=JSON";
                    var responseTask = client.GetAsync(Uri);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {


                        string jsonContent = result.Content.ReadAsStringAsync().Result;

                        CurrencyRates = JsonConvert.DeserializeObject<APIHeadRateResul>(jsonContent);
                        var cultureInfo = new CultureInfo("pl-PL");
                        CurrencyRates.rates.ToList().ForEach(rate =>
                        {
                            ResultCurrencyRates.Add(new CurrencyRate() { Currency = currency, RateToBaseCurrency = rate.mid, DateOfRate = DateTime.Parse(rate.effectiveDate, cultureInfo) });

                        });
                        break;
                    }
                    else //web api sent error response 
                    {
                        if (result.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            ResultCurrencyRates = new List<CurrencyRate>();
                            break;
                        }
                        else if (i == 2)
                            throw new Exception($"Error durring fetch data from  API:{client.BaseAddress + Uri}  StatusCode: {result.StatusCode}   Response: {result.Content}");
                    }
                }
            }
            return ResultCurrencyRates;
        }
        public int GetMaxRateRangeDays()
        {
            return 365;
        }
    }
}
