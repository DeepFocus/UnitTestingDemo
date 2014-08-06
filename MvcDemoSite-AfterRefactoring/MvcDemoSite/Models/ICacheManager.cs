namespace MvcDemoSite.Models
{
    public interface ICacheManager
    {
        void Store(string key, object data, int expireInSeconds);
        object Get(string key);
    }
}
