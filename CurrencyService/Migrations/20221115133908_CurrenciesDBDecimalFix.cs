using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyService.Migrations
{
    public partial class CurrenciesDBDecimalFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PLAIN SQL
            migrationBuilder.Sql("alter table dbo.CurrencyRates ALTER COLUMN RateToBaseCurrency decimal(20,10);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("alter table dbo.CurrencyRates ALTER COLUMN RateToBaseCurrency decimal(18,2);");
        }
    }
}
