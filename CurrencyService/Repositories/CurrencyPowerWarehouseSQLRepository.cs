using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories
{
    public class CurrencyPowerWarehouseSQLRepository : ICurrencyPowerWarehouseRepository
    {
        public int AddCurrencyIfNotExists(Currency currency)
        {
            throw new NotImplementedException();
        }

        public int AddCurrencyRate(Currency currency, DateTime date, decimal rate)
        {
            throw new NotImplementedException();
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
    }
}
