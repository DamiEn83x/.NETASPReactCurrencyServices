using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.Model;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyValueController : ControllerBase
    {


        [HttpGet("{Token}")]
        public ActionResult<int> GetDataProgress(int Token)
        {
            return Ok(0);
        }

        [HttpGet]
        public ActionResult<string> GetHelloWorld(int Token)
        {
            return Ok("Service is working");
        }

        [HttpGet]
        public ActionResult<IEnumerable<Currency>> GetCurrenciesTableA(int Token)
        {
            return Ok(Enumerable.Empty<Currency>());
        }

        [HttpGet]
        public ActionResult<IEnumerable<Currency>> GetCurrenciesTableAB(int Token)
        {
            return Ok(Enumerable.Empty<Currency>());
        }

        [HttpGet]
        public ActionResult<IEnumerable<CurrencyPowerChange>> GetCurrencyPowerChanges(DateTime DateFrom, DateTime DateTo, string CurrencyCode, IEnumerable<string> ReferenceCurrencies )
        {
            return Ok(Enumerable.Empty<CurrencyPowerChange>());
        }
    }
}
