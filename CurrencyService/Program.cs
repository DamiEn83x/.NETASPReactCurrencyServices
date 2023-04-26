using CurrencyService.BackgroundServices;
using CurrencyService.Data;
using CurrencyService.Repositories;
using CurrencyService.Repositories.Inrfaces;
using CurrencyService.Repositories.NBPAPI;
using CurrencyService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


 builder.Services.AddScoped<ICurrencyRatesRepository, APINBPCurrencyRatesRepository>();
 builder.Services.AddScoped<ICurrencyPowerWarehouseRepository, CurrencyPowerWarehouseSQLRepository>();
 builder.Services.AddScoped<ICurrencyProcessingService, CurrencyProcessingService>();

 builder.Services.Configure<CurrencyRatesFetcherBGServiceOptions>(builder.Configuration.GetSection("Extensions:"+
                                      CurrencyRatesFetcherBGServiceOptions.Position));
 builder.Services.Configure<APINBPCurrencyRatesRepositoryOptions>(builder.Configuration.GetSection("Extensions:" +
                           APINBPCurrencyRatesRepositoryOptions.Position));
builder.Services.Configure<CurrencyProcessingServiceOptions>(builder.Configuration.GetSection("Extensions:" +
                          CurrencyProcessingServiceOptions.Position));

builder.Services.AddHostedService<CurrencyRatesFetcherBGService>();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer( builder.Configuration.GetConnectionString("DefaultConnection").Replace("DB_HOST", Environment.GetEnvironmentVariable("DB_HOST"))));
builder.Logging.AddSeq();
var app = builder.Build();

    using(var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();



        bool databaseExists = false;
        while (!databaseExists)
        {
            databaseExists = dataContext.Database.CanConnect();
            if (!databaseExists)
            {
                Console.WriteLine("Waiting for database to come online...");
                Thread.Sleep(1000); // Sleep for 1 second
            }
        }
        dataContext.Database.EnsureCreated();
    }

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
