namespace MvcDemoSite.Models
{
    public interface ICurrencyRateManager
    {
        CurrencyRate GetRate(string code);
    }
}