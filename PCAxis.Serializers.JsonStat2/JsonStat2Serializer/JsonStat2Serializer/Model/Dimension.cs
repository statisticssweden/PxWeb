namespace PCAxis.Serializers.JsonStat2.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// JSON-stat follows a cube model: the values are organized in cells, and a cell is the intersection of various dimensions. The dimension property contains information about the dimensions of the dataset.
    /// http://json-stat.org/format/#dimension
    /// </summary>
    public class Dimension
    {
        private Dictionary<string, object> _extension;
        private string _label;
        private Model.Category _category;
        private Dictionary<string, object> _link;

        /// <summary>
        /// extension allows JSON-stat to be extended for particular needs. Providers are free to define where they include this property and what children are allowed in each case.
        /// http://json-stat.org/format/#extension
        /// </summary>
        [JsonProperty("extension", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Extension
        {
            get
            {
                return _extension;
            }

            set
            {
                _extension = value;
            }
        }

        /// <summary>
        /// It is used to assign a very short (one line) descriptive text to IDs at different levels of the response tree. It is language-dependent.
        /// http://json-stat.org/format/#label
        /// </summary>
        [JsonProperty("label")]
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = value;
            }
        }

        [JsonProperty("category")]
        internal Model.Category Category
        {
            get
            {
                return _category;
            }

            set
            {
                _category = value;
            }
        }

        /// <summary>
        /// https://json-stat.org/format/#link
        /// </summary>
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Link
        {
            get
            {
                return _link;
            }

            set
            {
                _link = value;
            }
        }
    }
}
