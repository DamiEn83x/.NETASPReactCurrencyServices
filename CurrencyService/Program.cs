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

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Logging.AddSeq();
var app = builder.Build();

    using(var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        var DBRepo=scope.ServiceProvider.GetRequiredService<ICurrencyPowerWarehouseRepository>();
        //automatic migration only for new application instance
        //for application updates use sql sripcs to avoid data lose
        if(DBRepo.DataBaseIsEmpty())
            dataContext.Database.Migrate();

    }

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
