using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class RatesDownload
    {
        public int RatesDownloadId { get; set; }
        public DateTime FetchDate {get; set;}
        public bool Successfull { get; set; }

        public string Error { get; set; }

    }
}
