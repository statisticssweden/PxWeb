namespace PCAxis.Serializers.JsonStat2.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// The JSON-stat format is a simple lightweight JSON format for data dissemination. It is based in a cube model that arises from the evidence that the most common form of data dissemination is the tabular form. In this cube model, datasets are organized in dimensions. Dimensions are organized in categories.
    /// </summary>
    public class JsonStat
    {
        private const string _version = "2.0";
        private const string _class = "dataset";
        private string _label;
        private string _source;
        private DateTime _updated;
        private string[] _id;
        private int[] _size;
        private Dictionary<string, object> _dimension;
        private double?[] _value;
        private Dictionary<int,string> _status;
        private Dictionary<string, string[]> _role;
        private Dictionary<string, object> _extension;
        private Dictionary<string, object[]> _link;

        /// <summary>
        /// JSON-stat supports several classes of responses. This particular implementation only supports 'dataset'
        /// http://json-stat.org/format/#class
        /// </summary>
        [JsonProperty("class")]
        public string Class
        {
            get
            {
                return _class;
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

        /// <summary>
        /// It contains a language-dependent short text describing the source of the dataset.
        /// http://json-stat.org/format/#source
        /// </summary>
        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
            }
        }



        /// <summary>
        /// It contains the update time of the dataset. It is a string representing a date in an ISO 8601 format recognized by the Javascript Date.parse method.
        /// http://json-stat.org/format/#updated
        /// </summary>
        [JsonProperty("updated")]
        public string Updated
        {
            get { return _updated.ToString("yyyy-MM-ddTHH:mm:ssZ"); }
            set { _updated = DateTime.Parse(value).ToUniversalTime(); }
        }

        /// <summary>
        /// It contains an ordered list of dimension IDs.
        /// http://json-stat.org/format/#id
        /// </summary>
        [JsonProperty("id")]
        public string[] Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// It contains the number (integer) of categories (possible values) of each dimension in the dataset. It has the same number of elements and in the same order as in id.
        /// http://json-stat.org/format/#size
        /// </summary>
        [JsonProperty("size")]
        public int[] Size
        {
            get
            {
                return _size;
            }

            set
            {
                _size = value;
            }
        }

        [JsonProperty("dimension")]
        public Dictionary<string, object> Dimension
        {
            get
            {
                return _dimension;
            }

            set
            {
                _dimension = value;
            }
        }

        /// <summary>
        /// It contains the data sorted according to the dataset dimensions. It usually takes the form of an array where missing values are expressed as nulls.
        /// http://json-stat.org/format/#value
        /// </summary>
        [JsonProperty("value")]
        public double?[] Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// It contains metadata at the observation level. When it takes an array form of the same size of value, it assigns a status to each data by position.
        /// http://json-stat.org/format/#status
        /// </summary>
        [JsonProperty("status",NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<int,string> Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
            }
        }

        /// <summary>
        /// It can be used to assign special roles to dimensions. At this moment, possible roles are: time, geo and metric. A role can be shared by several dimensions.
        /// http://json-stat.org/format/#role
        /// </summary>
        [JsonProperty("role",NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string[]> Role
        {
            get
            {
                return _role;
            }

            set
            {
                _role = value;
            }
        }

        /// <summary>
        /// It declares the JSON-stat version of the response. The goal of this property is to help clients parsing that particular response. This serializer only supports 2.0.
        /// http://json-stat.org/format/#version
        /// </summary>
        [JsonProperty("version")]
        public string Version
        {
            get
            {
                return _version;
            }
        }

        /// <summary>
        /// extension allows JSON-stat to be extended for particular needs. Providers are free to define where they include this property and what children are allowed in each case.
        /// http://json-stat.org/format/#extension
        /// </summary>
        [JsonProperty("extension",NullValueHandling = NullValueHandling.Ignore)]
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

        [JsonProperty("link",NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object[]> Link
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
