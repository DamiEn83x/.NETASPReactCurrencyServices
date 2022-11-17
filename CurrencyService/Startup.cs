using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.BackgroundServices;
using CurrencyService.Data;
using CurrencyService.Repositories;
using CurrencyService.Repositories.Inrfaces;
using CurrencyService.Repositories.NBPAPI;
using CurrencyService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CurrencyService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services)
        {
            services.AddScoped<ICurrencyRatesRepository, APINBPCurrencyRatesRepository>();
            services.AddScoped<ICurrencyPowerWarehouseRepository, CurrencyPowerWarehouseSQLRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<ICurrencyProcessingService, CurrencyProcessingService>();
            services.Configure<CurrencyRatesFetcherBGServiceOptions>(Configuration.GetSection("Extensions:"+
                                      CurrencyRatesFetcherBGServiceOptions.Position));
            services.Configure<APINBPCurrencyRatesRepositoryOptions>(Configuration.GetSection("Extensions:" +
                           APINBPCurrencyRatesRepositoryOptions.Position));
            services.Configure<CurrencyProcessingServiceOptions>(Configuration.GetSection("Extensions:" +
                          CurrencyProcessingServiceOptions.Position));

            services.AddHostedService<CurrencyRatesFetcherBGService>();

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = "ToDo API",
                    Description = "A simple example ASP.NET Core Web API"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout Ad Api");
            });

       
        }
    }
}
