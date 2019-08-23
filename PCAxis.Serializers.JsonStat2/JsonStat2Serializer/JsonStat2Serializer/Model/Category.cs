namespace PCAxis.Serializers.JsonStat2.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// It is used to describe the possible values of a dimension.
    /// http://json-stat.org/format/#category
    /// </summary>
    public class Category
    {
        private Dictionary<string, int> _index = new Dictionary<string, int>();
        private Dictionary<string, string> _label = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<string, object>> _unit;

        /// <summary>
        /// It is used to order the possible values (categories) of a dimension. The order of the categories and the order of the dimensions themselves determine the order of the data in the value array. While the dimensions’ order has only this functional role (and therefore any order chosen by the provider is valid), the categories’ order has also a presentation role: it is assumed that the categories are sorted in a meaningful order and that the consumer can rely on it when displaying the information. For example, categories in dimensions with a time role are assumed to be in chronological order
        /// </summary>
        [JsonProperty("index")]
        public Dictionary<string, int> Index
        {
            get
            {
                return _index;
            }

            set
            {
                _index = value;
            }
        }

        [JsonProperty("label")]
        public Dictionary<string, string> Label
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

        [JsonProperty("unit",NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, object>> Unit
        {
            get
            {
                return _unit;
            }

            set
            {
                _unit = value;
            }
        }
        public Category()
        {
            
        }

    }
}
