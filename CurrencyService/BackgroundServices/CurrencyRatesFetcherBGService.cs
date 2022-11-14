using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyService.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyService.BackgroundServices
{
    public class CurrencyRatesFetcherBGServiceOptions
    {
        public const string Position = "CurrencyRatesFetcherBGService";
        public int FetchTimeHour { get; set; }
    }
    public class CurrencyRatesFetcherBGService : BackgroundService
    {
        private readonly ILogger<CurrencyRatesFetcherBGService> _logger;
        private IServiceScopeFactory _scopeFactory;
        private DateTime _LastFetchTime;
        private int _FetchHour;

        public CurrencyRatesFetcherBGService(IServiceScopeFactory scopeFactory, ILogger<CurrencyRatesFetcherBGService> logger, IOptions<CurrencyRatesFetcherBGServiceOptions> options)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _LastFetchTime= new DateTime();
            _FetchHour = ((CurrencyRatesFetcherBGServiceOptions)options.Value).FetchTimeHour;
            Console.WriteLine($"CurrencyRatesFetcherBGService fetch hour: {_FetchHour}");

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork(stoppingToken);
                await Task.Delay(60000); // 5 minutes
            }
        }


        private async Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                ICurrencyProcessingService CurrencyProcessService= scope.ServiceProvider.GetRequiredService<ICurrencyProcessingService>();
                DateTime CurrentTime = DateTime.Now;
                if ((CurrentTime.Date > _LastFetchTime.Date) && (CurrentTime.Hour >= _FetchHour))
                {
                    if (await CurrencyProcessService.FetchandSaveNewDataFromCurrencyRatesProvider())
                        _LastFetchTime = CurrentTime;
                }
            }

        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
