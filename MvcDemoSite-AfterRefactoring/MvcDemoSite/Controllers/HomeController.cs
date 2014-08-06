using System;
using System.Web.Mvc;
using MvcDemoSite.Models;

namespace MvcDemoSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger;
        private readonly ICurrencyRateManager _currencyRateManager;
        private readonly ICacheManager _cacheManger;

        public HomeController(ICacheManager cacheManager, 
                              ICurrencyRateManager currencyRateManager,
                              ILog logger)
        {
            _cacheManger = cacheManager;
            _currencyRateManager = currencyRateManager;
            _logger = logger;
        }

        [Route("~/")]
        // Return rate from cache if not found
        // try grab it from web service
        // if unable to do so, log it and return null
        public ViewResult Index()
        {
            const string cacheKey = "rates.USD-ERU";
            var cachedRate = _cacheManger.Get(cacheKey);
            
            if (cachedRate == null)
            {
                try
                {   
                    cachedRate = _currencyRateManager.GetRate("USDEUR=X");
                    _cacheManger.Store(cacheKey, cachedRate, 10);
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