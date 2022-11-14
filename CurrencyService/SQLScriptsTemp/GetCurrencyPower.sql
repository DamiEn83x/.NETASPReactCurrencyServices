begin
declare @Datefrom dateTime = '2022-01-01',
		@DateTo dateTime ='2022-11-15',
	    @CurrencyCode nvarchar(max)='PLN',
		@RefCurrencyCodes nvarchar(max) ='EUR,USD',
		@in_string VARCHAR(MAX),
		@delimiter VARCHAR(1)=','


		select max(cr2.DateOfRate)  from CurrencyRates cr2 where  cr2.DateOfRate<=@Datefrom 

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
		
		select cr.DateOfRate,sum(r.bucketamount)/sum(1/cr.RateToBaseCurrency) as valueindex from #ReferenceList  r join  Currencies c on r.Code=c.code join  CurrencyRates cr on  cr.CurrencyId=cr.CurrencyId group by cr.DateOfRate

		drop table #ReferenceList

end;