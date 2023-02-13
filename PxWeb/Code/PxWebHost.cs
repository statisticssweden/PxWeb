using Microsoft.AspNetCore.Hosting;
using Px.Abstractions.Interfaces;

namespace PxWeb.Code
{
    public class PxWebHost : IPxHost
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PxWebHost(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        
        public string RootPath => _hostingEnvironment.WebRootPath;
    }
}
