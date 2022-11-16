﻿// <auto-generated />
using System;
using CurrencyService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CurrencyService.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CurrencyService.Model.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Desription");

                    b.Property<string>("Table");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("CurrencyService.Model.CurrencyPowerChange", b =>
                {
                    b.Property<int>("CurrencyPowerChangeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("PowerIndicator");

                    b.HasKey("CurrencyPowerChangeId");

                    b.ToTable("CurrencyPawerChanges");
                });

            modelBuilder.Entity("CurrencyService.Model.CurrencyRate", b =>
                {
                    b.Property<int>("CurrencyRateId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrencyId");

                    b.Property<DateTime>("DateOfRate");

                    b.Property<decimal>("RateToBaseCurrency");

                    b.HasKey("CurrencyRateId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("CurrencyRates");
                });

            modelBuilder.Entity("CurrencyService.Model.RatesDownload", b =>
                {
                    b.Property<int>("RatesDownloadId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Error");

                    b.Property<DateTime>("FetchDate");

                    b.Property<bool>("Successfull");

                    b.HasKey("RatesDownloadId");

                    b.ToTable("RatesDownloads");
                });

            modelBuilder.Entity("CurrencyService.Model.CurrencyRate", b =>
                {
                    b.HasOne("CurrencyService.Model.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");
                });
#pragma warning restore 612, 618
        }
    }
}
