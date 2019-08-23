namespace PCAxis.Serializers.JsonStat.Model
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class JsonStatDatasetBase
    {
        private string _label;
        private string _source;
        private DateTime _updated;
        private double?[] _value;
        private Dictionary<string, object> _dimension;

        /// <summary>
        /// JSON-stat follows a cube model: the values are organized in cells, 
        /// and a cell is the intersection of various dimensions. The dimension property contains information about the dimensions of the dataset.
        /// http://json-stat.org/format/#dimension
        /// </summary>
        public Dictionary<string, object> dimension
        {
            get { return _dimension; }
            set { _dimension = value; }
        }
        /// <summary>
        /// http://json-stat.org/format/#label
        /// </summary>
        public string label
        {
            get { return _label; }
            set { _label = value; }
        }
        /// <summary>
        /// It contains a language-dependent short text describing the source of the dataset.
        /// http://json-stat.org/format/#source
        /// </summary>
        public string source
        {
            get { return _source; }
            set { _source = value; }
        }
        /// <summary>
        /// It contains the update time of the dataset. It is a string representing a date in an ISO 8601 format recognized by the Javascript Date.parse method.
        /// http://json-stat.org/format/#updated
        /// </summary>
        public string updated
        {
            get { return _updated.ToString("yyyy-MM-ddTHH:mm:ssZ"); }
            set { _updated = DateTime.Parse(value).ToUniversalTime(); }
        }
        /// <summary>
        /// It contains the data sorted according to the dataset dimensions. It usually takes the form of an array where missing values are expressed as nulls.
        /// http://json-stat.org/format/#value
        /// </summary>
        public double?[] value
        {
            get { return this._value; }
            set { this._value = value; }
        }

        /// <summary>
        /// Base JSON-stat dataset-field class without observation status field.
        /// </summary>
        public JsonStatDatasetBase(int matrixSize)
        {
            this.value = new double?[matrixSize];
        }
    }
}
