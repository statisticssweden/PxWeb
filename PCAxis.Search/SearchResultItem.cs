using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PCAxis.Search
{
    /// <summary>
    /// Represents a table found by a search
    /// </summary>
    public class SearchResultItem
    {
        /// <summary>
        /// Table
        /// </summary>
        [JsonProperty("id")]
        public string Table { get; set; }

        /// <summary>
        /// Table path within its database
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// Table title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Search score (relevance of search)
        /// </summary>
        [JsonProperty("score")]
        public float Score { get; set; }

        /// <summary>
        /// The date the table was published
        /// </summary>
        [JsonProperty("published")]
        public DateTime Published { get; set; }
    }
}
