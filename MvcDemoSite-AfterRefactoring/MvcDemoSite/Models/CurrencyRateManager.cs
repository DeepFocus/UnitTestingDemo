using System;
using System.Net;

namespace MvcDemoSite.Models
{
    public class CurrencyRateManager : ICurrencyRateManager
    {
        private readonly ILog _logger;

        public CurrencyRateManager(ILog logger)
        {
            _logger = logger;
        }

        public CurrencyRate GetRate(string code)
        {
            string result;
            using (var client = new WebClient())
            {
                result = client.DownloadString(
                    string.Format("http://download.finance.yahoo.com/d/quotes.csv?s={0}&f=sl1d1t1c1ohgv&e=.csv", code));
            }
            _logger.Trace(result);
            return new CurrencyRate
            {
                Title = "USD to EUR",
                Rate = Decimal.Parse(result.Split(new[] {','})[1]),
                LastUpdated = DateTime.Now
            };
        }
    }
}