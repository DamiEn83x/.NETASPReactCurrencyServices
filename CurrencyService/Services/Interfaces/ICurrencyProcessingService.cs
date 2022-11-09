using System.Threading.Tasks;

namespace CurrencyService.Services
{
    public interface ICurrencyProcessingService
    {
        Task FetchandSaveNewDataFromCurrencyRatesProvider();
    }
}