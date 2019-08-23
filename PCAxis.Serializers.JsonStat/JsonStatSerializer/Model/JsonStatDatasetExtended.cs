namespace PCAxis.Serializers.JsonStat.Model
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class JsonStatDatasetExtended : JsonStatDatasetBase
    {
        /// <summary>
        /// JSON-stat dataset field class. Extended with a status field for observation status
        /// </summary>
        public JsonStatDatasetExtended(int matrixSize) : base(matrixSize) { }

        private object _status;

        /// <summary>
        /// It contains metadata at the observation level.
        /// http://json-stat.org/format/#status
        /// </summary>
        public object status
        {
            get { return this._status; }
            set { this._status = value; }
        }
    }
}