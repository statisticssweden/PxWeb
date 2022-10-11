namespace PxWeb
{
    public class HttpResponse
    {
        public string content { get; set; }
        public string contentType { get; set; }
        public HttpResponse(string content, string responseType)
        {
            this.content = content;
            this.contentType = responseType;
        }
    }
}
