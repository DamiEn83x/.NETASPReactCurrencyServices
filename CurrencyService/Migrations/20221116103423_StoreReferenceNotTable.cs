using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyService.Migrations
{
    public partial class StoreReferenceNotTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Table",
                table: "Currencies");

            migrationBuilder.AddColumn<bool>(
                name: "BaseCurrency",
                table: "Currencies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ReferenceCurrency",
                table: "Currencies",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseCurrency",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "ReferenceCurrency",
                table: "Currencies");

            migrationBuilder.AddColumn<string>(
                name: "Table",
                table: "Currencies",
                nullable: true);
        }
    }
}
