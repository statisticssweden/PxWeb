using log4net;
using Newtonsoft.Json;
using PCAxis.Paxiom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PCAxis.Serializers.JsonStat2
{
    public class JsonStat2Serializer : IPXModelStreamSerializer
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(JsonStat2Serializer));

        // Reserved fields in JSON-stat (in no particular order)
        private const string TIME = "time";
        private const string METRIC = "metric";
        private const string UNIT = "unit";
        private const string BASE = "base";
        private const string GEO = "geo";
        private const string SIZE = "size";
        private const string DECIMALS = "decimals";

        private Dictionary<double, string> BuildDataSymbolMap(PXMeta meta)
        {
            var dataSymbolMap = new Dictionary<double, string>();

            // Would have been handy to actually have this as a map in PAxiom.
            dataSymbolMap.Add(PXConstant.DATASYMBOL_1, string.IsNullOrEmpty(meta.DataSymbol1) ? PXConstant.DATASYMBOL_1_STRING : meta.DataSymbol1);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_2, string.IsNullOrEmpty(meta.DataSymbol2) ? PXConstant.DATASYMBOL_2_STRING : meta.DataSymbol2);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_3, string.IsNullOrEmpty(meta.DataSymbol3) ? PXConstant.DATASYMBOL_3_STRING : meta.DataSymbol3);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_4, string.IsNullOrEmpty(meta.DataSymbol4) ? PXConstant.DATASYMBOL_4_STRING : meta.DataSymbol4);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_5, string.IsNullOrEmpty(meta.DataSymbol5) ? PXConstant.DATASYMBOL_5_STRING : meta.DataSymbol5);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_6, string.IsNullOrEmpty(meta.DataSymbol6) ? PXConstant.DATASYMBOL_6_STRING : meta.DataSymbol6);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_7, string.IsNullOrEmpty(meta.DataSymbol7) ? PXConstant.DATASYMBOL_7_STRING : meta.DataSymbol7);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_NIL, string.IsNullOrEmpty(meta.DataSymbolNIL) ? PXConstant.DATASYMBOL_NIL_STRING : meta.DataSymbolNIL);

            return dataSymbolMap;
        }

        private void ExtractValueAndStatus(PXMeta meta, double[] matrix, out double?[] value, out Dictionary<int, string> status)
        {
            value = new double?[matrix.Length];
            status = new Dictionary<int, string>();

            var datasymbol = BuildDataSymbolMap(meta);

            for (var i = 0; i < matrix.Length; i++)
            {
                string symbol = null;

                if (datasymbol.TryGetValue(matrix[i], out symbol))
                {
                    value[i] = null;
                    status.Add(i, symbol);
                }
                else
                {
                    value[i] = matrix[i];
                }
            }
        }

        public string BuildJsonStructure(PXModel model)
        {

            var jsonStat = new Model.JsonStat();

            var value = new double?[model.Data.Matrix.Length];
            var status = new Dictionary<int, string>();
            var id = new List<string>();
            var size = new List<int>();

            // Role collectors
            var roleTimeList = new List<string>();
            var roleMetricList = new List<string>();
            var roleGeoList = new List<string>();

            ExtractValueAndStatus(model.Meta, model.Data.Matrix, out value, out status);

            jsonStat.Value = value;
            jsonStat.Status = status.Count == 0 ? null : status;
            jsonStat.Source = model.Meta.Source;
            jsonStat.Label = model.Meta.Title;
            jsonStat.Updated = DateTime.Now;

            // Dimension
            jsonStat.Dimension = new Dictionary<string, object>();

            for (var i = 0; i < model.Meta.Variables.Count; i++)
            {
                var variable = model.Meta.Variables[i];

                var dimension = new Model.Dimension();
                dimension.Label = variable.Name;

                // Category
                var category = new Model.Category();
                var unit = new Dictionary<string, Dictionary<string, object>>();

                for (var j = 0; j < variable.Values.Count; j++)
                {
                    var varvalue = variable.Values[j];

                    category.Label.Add(varvalue.Code, varvalue.Value);
                    category.Index.Add(varvalue.Code, j);

                    if (variable.IsContentVariable)
                    {
                        var unitContent = new Dictionary<string, object>();

                        if (varvalue.ContentInfo != null)
                        {
                            unitContent.Add(BASE, varvalue.ContentInfo.Units);

                        }
                        else
                        {
                            _logger.Warn("Category" + varvalue.Code + " lacks ContentInfo. Unit not set");
                        }

                        var decimals = (varvalue.HasPrecision()) ? varvalue.Precision : model.Meta.ShowDecimals;

                        unitContent.Add(DECIMALS, decimals);
                        unit.Add(varvalue.Code, unitContent);

                        category.Unit = unit;
                    }
                }

                dimension.Category = new Model.Category();
                dimension.Category = category;

                jsonStat.Dimension.Add(variable.Code, dimension);

                size.Add(variable.Values.Count);
                id.Add(variable.Code);

                // Role
                if (variable.IsTime)
                {
                    roleTimeList.Add(variable.Code);
                }

                if (variable.IsContentVariable)
                {
                    roleMetricList.Add(variable.Code);
                }

                if (String.IsNullOrEmpty(variable.Map) == false)
                {
                    roleGeoList.Add(variable.Code);
                }
            }

            // Id
            jsonStat.Id = new string[id.Count];
            jsonStat.Id = id.ToArray();

            // Size
            jsonStat.Size = new int[size.Count];
            jsonStat.Size = size.ToArray();

            // Role
            jsonStat.Role = new Dictionary<string, string[]>();

            if (roleTimeList.Count > 0) { jsonStat.Role.Add(TIME, roleTimeList.ToArray()); }
            if (roleMetricList.Count > 0) { jsonStat.Role.Add(METRIC, roleMetricList.ToArray()); }
            if (roleGeoList.Count > 0) { jsonStat.Role.Add(GEO, roleGeoList.ToArray()); }

            string result = JsonConvert.SerializeObject(jsonStat, Formatting.Indented);

            return result;
        }

        public void Serialize(PXModel model, Stream stream)
        {
            var result = BuildJsonStructure(model);

            byte[] jsonData = Encoding.UTF8.GetBytes(result);
            stream.Write(jsonData, 0, jsonData.Length);
        }

        public void Serialize(PXModel model, string path)
        {
            var result = BuildJsonStructure(model);
            var encoding = new UTF8Encoding();
            var fileName = model.Meta.MainTable + ".json";

            File.WriteAllText(path + fileName, result, encoding);
        }
    }

}
