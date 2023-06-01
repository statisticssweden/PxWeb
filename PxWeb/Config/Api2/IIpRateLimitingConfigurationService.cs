namespace PxWeb.Config.Api2
{
    public interface IIpRateLimitingConfigurationService
    {
        IpRateLimitingConfigurationOptions GetConfiguration();
    }
}
