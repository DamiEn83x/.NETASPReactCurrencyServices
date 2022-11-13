using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Repositories.NBPAPI
{
    public class APICurrencyResult
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }
    }

    public class APITableResultObject
    {
         public string table { get; set; }
         public string no { get; set; }
         public string effectiveDate { get; set; }
        public IList<APICurrencyResult> rates { get; set; }

    }

    public class APIHeadRateResul
    {
        public string table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public IList<APICurrencyRateResult> rates { get; set; }

    }
    public class APICurrencyRateResult
    {
        public decimal mid { get; set; }
        public string effectiveDate { get; set; }
        public string no { get; set; }
    }
}
