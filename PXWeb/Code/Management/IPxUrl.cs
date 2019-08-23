using System;
using System.Collections.Generic;
using PCAxis.Web.Core.Management;

namespace PXWeb
{
    public interface IPxUrlProvider
    {
        IPxUrl Create(params PCAxis.Web.Core.Management.LinkManager.LinkItem[] links);
        IPxUrl Create();
    }

    public class PxUrlProvider : IPxUrlProvider
    {
        public IPxUrl Create()
        {
            return new PxUrl();
        }

        public IPxUrl Create(params LinkManager.LinkItem[] links)
        {
            return new PxUrl(links);
        }
    }

    public interface IPxUrl
    {
        string Database { get; set; }
        string Language { get; set; }
        string Layout { get; set; }
        string Path { get; set; }
        List<KeyValuePair<string, string>> QuerystringParameters { get; }
        string Table { get; set; }
        string TablePath { get; }
        string View { get; set; }
        void AddParameter(string key, string value);
    }
}