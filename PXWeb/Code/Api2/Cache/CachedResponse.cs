namespace PxWeb.Code.Api2.Cache
{
    public class CachedResponse
    {
        public byte[] content { get; set; }
        public string contentType { get; set; }
        public int responseCode { get; set; }
        public CachedResponse(byte[] content, string responseType, int responseCode)
        {
            this.content = content;
            contentType = responseType;
            this.responseCode = responseCode;
        }
    }
}
