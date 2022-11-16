using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    [NotMapped]
    public class CurrencyPowerChange
    {
        public int CurrencyPowerChangeId { get; set; }
        public DateTime Date { get; set; }
       public decimal PowerIndicator { get; set;}
    }
}
