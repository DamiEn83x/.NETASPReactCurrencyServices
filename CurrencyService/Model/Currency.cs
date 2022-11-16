﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Model
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Code { get; set; }
        public string Desription { get; set; }
        public bool ReferenceCurrency { get; set; }
        public bool BaseCurrency { get; set;  }

    }
}
