namespace CurrencyService.Model
{
    public class RatesDownload
    {
        public int RatesDownloadId { get; set; }
        public DateTime FetchDate {get; set;}

        public DateTime PublishedDate  {get; set;}
        public bool Successfull { get; set; }

        public string? Error { get; set; }

    }
}
