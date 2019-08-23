using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace PCAxis.Api
{
    /// <summary>
    /// Parses Options in the querystring of the url
    /// </summary>
    public class Options
    {
        /// <summary>
        /// If results should be pretty printed
        /// </summary>
        public bool PrettyPrint { get; set; }

        /// <summary>
        /// Search query for finding tables using the search function
        /// </summary>
        public string SearchQuery { get; set; }

        /// <summary>
        /// Filter defining which fields to look in when using the search function. Comma separated list of field names.
        /// </summary>
        public string SearchFilter { get; set; }       

        /// <summary>
        /// Constructor
        /// </summary>
        public Options() { }

        /// <summary>
        /// Parses a query string
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public Options(NameValueCollection queryString)
        {
            foreach (string key in queryString.Keys)
            {
                if(string.IsNullOrEmpty(key)) continue;
                switch (key.ToLower())
                {
                    case "prettyprint":
                        PrettyPrint = (queryString[key] == "true");
                        break;
                    case "query":
                        SearchQuery = queryString[key];
                        break;
                    case "filter":
                        SearchFilter = queryString[key];
                        break;                   
                    default:
                        break;
                }
            }
        }
    }
}