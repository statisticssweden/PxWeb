namespace PCAxis.Serializers.JsonStat.Model
{
    using System;

    /// <summary>
    /// JSON-stat base class. Contains only one field for dataset. The naming of this field is set to default 'dataset', but
    /// could be anything we like. 
    /// http://json-stat.org/format/#dataset
    /// </summary>
    [Serializable]
    public class JsonStat
    {
        private object _dataset;

        public object dataset
        {
            get { return _dataset; }
            set { _dataset = value; }
        }

        public JsonStat()  { }

    }
}
