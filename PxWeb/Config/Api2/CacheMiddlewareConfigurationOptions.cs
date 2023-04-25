namespace PxWeb.Config.Api2
{
    public class CacheMiddlewareConfigurationOptions
    {
        public int CacheTime { get; set; } = 10;

        /// <summary>
        /// Size in bytes before body is written to file default is 30K
        /// </summary>
        public int BufferThreshold { get; set; } = 30 * 1024;
    }
}
