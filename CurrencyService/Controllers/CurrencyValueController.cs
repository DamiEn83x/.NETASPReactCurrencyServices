using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.Model;
using CurrencyService.Repositories.Inrfaces;
using CurrencyService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyValueController : ControllerBase
    {
        private ICurrencyProcessingService _CurrencyProcessingService;
        public CurrencyValueController(ICurrencyProcessingService CurrencyProcessingService)
        {
            _CurrencyProcessingService = CurrencyProcessingService;
        }

        [HttpGet("/GetDataProgress/{Token}")]
        public ActionResult<int> GetDataProgress(int Token)
        {
            return Ok(0);
        }

        [HttpGet("/GetHelloWorld/")]
        public ActionResult<string> GetHelloWorld()
        {
            return Ok("Service is working");
        }

        [HttpGet("/GetRefCurrencies/")]
        public ActionResult<IEnumerable<Currency>> GetRefCurrencies()
        {
            return Ok(_CurrencyProcessingService.GetReferenceCurrencies().Select(a => CurrencyDTO.FromCurrency(a)).ToList());
        }

        [HttpGet("/GetAllCurrencies/")]
        public ActionResult<IEnumerable<CurrencyDTO>> GetAllCurrencies()
        {
            return Ok(_CurrencyProcessingService.GetAllCurrencies().Select(a =>CurrencyDTO.FromCurrency(a)).ToList());
        }

        [HttpGet("/GetCurrencyPowerChanges/{DateFrom}/{DateTo}/{CurrencyCode}/{ReferenceCurrencies}")]
        public ActionResult<IEnumerable<CurrencyPowerChange>> GetCurrencyPowerChanges(DateTime DateFrom, DateTime DateTo, string CurrencyCode,string ReferenceCurrencies)
        {
            return Ok(_CurrencyProcessingService.GetCurrencyPowerRange(DateFrom,DateTo,CurrencyCode,ReferenceCurrencies).Select(a => CurrencyPowerChangeDTO.CurrencyPowerChange(a)).ToList());
        }

        [HttpGet("/GetCurrencyPowerChanges/{DateFrom}/{DateTo}/{CurrencyCode}")]
        public ActionResult<IEnumerable<CurrencyPowerChange>> GetCurrencyPowerChanges(DateTime DateFrom, DateTime DateTo, string CurrencyCode)
        {
            return Ok(_CurrencyProcessingService.GetCurrencyPowerRange(DateFrom, DateTo, CurrencyCode).Select(a => CurrencyPowerChangeDTO.CurrencyPowerChange(a)).ToList());
        }
    }
}
