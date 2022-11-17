using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyService.Migrations
{
    public partial class GetCurrencyPowerStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[SelectCurrencyPowers]
                    @Datefrom dateTime,
					@DateTo dateTime,
					@CurrencyCode nvarchar(max),
					@RefCurrencyCodes nvarchar(max)
                    AS
                    begin
                    declare 
		                    @in_string VARCHAR(MAX),
		                    @delimiter VARCHAR(1)=','

		                    set @in_string = @RefCurrencyCodes;

		                    create table #ReferenceList (
			                    code varchar( 10 ) not null,
			                    bucketamount decimal(20,10) null
		                    )
		                     WHILE LEN(@in_string) > 0
                            BEGIN
                                INSERT INTO #ReferenceList(code)
                                SELECT left(@in_string, charindex(@delimiter, @in_string+',') -1) as tuple
    
                                SET @in_string = stuff(@in_string, 1, charindex(@delimiter, @in_string + @delimiter), '')
                            end
		                    update ref set ref.bucketamount=(1/cr.RateToBaseCurrency)  from  #ReferenceList as ref join Currencies c on c.Code=ref.code join CurrencyRates cr on cr.CurrencyId=c.CurrencyId and cr.DateOfRate=@Datefrom
		

		                    select CAST (ROW_NUMBER() OVER(ORDER BY cr.DateOfRate ASC) AS INT) AS CurrencyPowerChangeId,cr.DateOfRate as Date,sum(r.bucketamount)/sum(1/cr.RateToBaseCurrency) as PowerIndicator from #ReferenceList  r join  Currencies c on r.Code=c.code join  CurrencyRates cr on  cr.CurrencyId=c.CurrencyId  where cr.DateOfRate>=@Datefrom and cr.DateOfRate<=@DateTo group by cr.DateOfRate order by DateOfRate
	
		                    drop table #ReferenceList

                    end;";

            migrationBuilder.Sql(sp);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
