using CurrencyService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Data
{
    public class DataContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder  modelBuilder)
        {
            modelBuilder.Entity<CurrencyRate>()
                        .Property(p => p.RateToBaseCurrency)
                        .HasPrecision(20,10); // or whatever your schema specifies
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrencyRates { get; set; }

        public DbSet<RatesDownload> RatesDownloads { get; set; }

        public DbSet<CurrencyPowerChange> CurrencyPawerChanges { get; set; }

    }
}
