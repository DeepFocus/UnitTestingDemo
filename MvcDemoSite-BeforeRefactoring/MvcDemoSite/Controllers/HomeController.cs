using System;
using System.Net;
using System.Runtime.Caching;
using System.Web.Mvc;
using MvcDemoSite.Models;
using NLog;

namespace MvcDemoSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        [Route("~/")]
        public ActionResult Index()
        {
            ObjectCache cache = MemoryCache.Default;
            const string key = "currency.rates.usd-eur";

            var cachedRate = cache.Get(key) as CurrencyRate;
            if (cachedRate == null)
            {
                var expireIn10Min = new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(10))
                };

                try
                {
                    string result;
                    using (var client = new WebClient())
                    {
                        result = client.DownloadString(
                            "http://download.finance.yahoo.com/d/quotes.csv?s=USDEUR=X&f=sl1d1t1c1ohgv&e=.csv");
                        _logger.Trace(result);
                    }
                    cachedRate = new CurrencyRate
                    {
                        Title = "USD to EUR",
                        Rate = Decimal.Parse(result.Split(new[] {','})[1]),
                        LastUpdated = DateTime.Now
                    };
                    cache.Add(key, cachedRate, expireIn10Min);
                }
                catch (Exception e)
                {
                    _logger.Error("Currency update failed", e);
                }
            }

            return View(cachedRate);
        }
	}
}