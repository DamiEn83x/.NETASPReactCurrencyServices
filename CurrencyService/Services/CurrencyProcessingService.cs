using CurrencyService.Repositories.Inrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public class CurrencyProcessingService : ICurrencyProcessingService
    {
        private ICurrencyRatesRepository _CurrencyRatesRepository;
        private ICurrencyPowerWarehouseRepository _CurrencyPowerWarehouseRepository;
        public CurrencyProcessingService(ICurrencyRatesRepository CurrencyRatesRepository , ICurrencyPowerWarehouseRepository CurrencyPowerWarehouseRepository)
        {
            _CurrencyRatesRepository = CurrencyRatesRepository;
            _CurrencyPowerWarehouseRepository = CurrencyPowerWarehouseRepository;
        }

        public async Task FetchandSaveNewDataFromCurrencyRatesProvider()
        {
            Console.WriteLine("Fetching data from Currency repository");

        }
    }
}
