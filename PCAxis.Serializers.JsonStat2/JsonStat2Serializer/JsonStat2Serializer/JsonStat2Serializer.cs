using System;
using log4net;
using Newtonsoft.Json;
using PCAxis.Paxiom;
using PCAxis.Metadata;
using PCAxis.Paxiom.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;


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
        private const string EXTENSION = "extension";
        private const string DESCRIBEDBY = "describedby";

		//Field in JSON-Stat, used for PX extention 
		private const string PX = "px";

		private MetaLinkManager metaLinkManager = new MetaLinkManager();

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
            dataSymbolMap.Add(PXConstant.DATASYMBOL_7, string.IsNullOrEmpty(meta.DataSymbolSum) ? PXConstant.DATASYMBOL_7_STRING : meta.DataSymbolSum); //Strange code due to lagacy which has been addressed but not fixed
            dataSymbolMap.Add(PXConstant.DATASYMBOL_NIL, string.IsNullOrEmpty(meta.DataSymbolNIL) ? PXConstant.DATASYMBOL_NIL_STRING : meta.DataSymbolNIL);

            return dataSymbolMap;
        }

  
        
        private void ExtractValueAndStatus(PXModel model, out double?[] value, out Dictionary<int, string> status)
        {
            int matrixSize = model.Data.MatrixColumnCount * model.Data.MatrixRowCount;
            value = new double?[matrixSize];
            var buffer = new double[model.Data.MatrixColumnCount];
            var dataSymbolMap = BuildDataSymbolMap(model.Meta);
            var formatter = new DataFormatter(model);
            string note = string.Empty;
            string dataNote = string.Empty;
            status = new Dictionary<int, string>();
            int n = 0;
            var numberFormatInfo = new System.Globalization.NumberFormatInfo();
            for (int i = 0; i < model.Data.MatrixRowCount; i++)
            {
                model.Data.ReadLine(i, buffer);
                for (int j = 0; j < model.Data.MatrixColumnCount; j++)
                {
                    string symbol = null;

                    if (dataSymbolMap.TryGetValue(buffer[j], out symbol))
                    {
                        value[n] = null;
                        status.Add(n, symbol);
                    }
                    else
                    {
                        value[n] = Convert.ToDouble(formatter.ReadElement(i, j, ref note, ref dataNote, ref numberFormatInfo), numberFormatInfo);
                        if (!string.IsNullOrEmpty(dataNote))
                        {
                            status.Add(n, dataNote);
                        }
                    }
                    n++;
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

            ExtractValueAndStatus(model, out value, out status);

            jsonStat.Value = value;
            jsonStat.Status = status.Count == 0 ? null : status;
            jsonStat.Source = model.Meta.Source;
            jsonStat.Label = model.Meta.Title;


            if (model.Meta.ContentVariable != null && model.Meta.ContentVariable.Values.Count > 0)
            {
                var lastUpdatedContentsVariable = model.Meta.ContentVariable.Values.OrderByDescending(x => x.ContentInfo.LastUpdated).FirstOrDefault();
                jsonStat.Updated = lastUpdatedContentsVariable.ContentInfo.LastUpdated.PxDateStringToDateTime().ToString();
            }
            else
            {
                jsonStat.Updated = model.Meta.CreationDate.PxDateStringToDateTime().ToString();
            }

            // Dimension
            jsonStat.Dimension = new Dictionary<string, object>();

            for (var i = 0; i < model.Meta.Variables.Count; i++)
            {
                var variable = model.Meta.Variables[i];
                var link = new Dictionary<string, object>();
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
                var extensions = GetAllSerializedMetaIdsForVariable(variable);
                if (extensions.Count > 0)
                {
                    link.Add(DESCRIBEDBY, new List<object> { extensions });
                    dimension.Link = link;
                }
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
                if (variable.VariableType != null)
                {
                    if (variable.VariableType.ToUpper() == "G" || (variable.Map != null))
                    {
                        roleGeoList.Add(variable.Code);
                    }
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

			//Extension, PX 
			if (model.Meta.InfoFile != null || model.Meta.TableID != null || model.Meta.Decimals != -1)
			{
				jsonStat.Extension = new Dictionary<string, object>();
				var px = new Model.Px();

				px.infofile = model.Meta.InfoFile;
				px.tableid = model.Meta.TableID;
				//If not Showdecimal has value use Decimal
				var decimals = model.Meta.ShowDecimals < 0 ? model.Meta.Decimals : model.Meta.ShowDecimals;
				px.decimals = decimals;

				jsonStat.Extension.Add(PX, px);
			}


			if (roleTimeList.Count > 0) { jsonStat.Role.Add(TIME, roleTimeList.ToArray()); }
            if (roleMetricList.Count > 0) { jsonStat.Role.Add(METRIC, roleMetricList.ToArray()); }
            if (roleGeoList.Count > 0) { jsonStat.Role.Add(GEO, roleGeoList.ToArray()); }



            // override converter to stop adding ".0" after interger values.
            string result = JsonConvert.SerializeObject(jsonStat,new DecimalJsonConverter());
 
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

        private Dictionary<string, object> GetAllSerializedMetaIdsForVariable(Variable variable)
        {
            var metaIds = new Dictionary<string, object>();
            var extension = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(variable.MetaId))
            {
                metaIds.Add(variable.Code, SerializeMetaIds(variable.MetaId));
            }
            foreach (var value in variable.Values)
            {
                if (!string.IsNullOrWhiteSpace(value.MetaId))
                {
                    metaIds.Add(value.Code, SerializeMetaIds(value.MetaId));
                }
            }
            if (metaIds.Count > 0)
            {
                extension.Add(EXTENSION, metaIds);
            }
            return extension;
        }

        private string SerializeMetaIds(string metaId)
        {
            var metaIds = metaId.Split(metaLinkManager.GetSystemSeparator(), StringSplitOptions.RemoveEmptyEntries);
            List<string> metaIdsAsString = new List<string>();
            foreach (var meta in metaIds)
            {
                var metaLinks = meta.Split(metaLinkManager.GetParamSeparator(), StringSplitOptions.RemoveEmptyEntries);
                if (metaLinks.Length > 0)
                {
                    metaIdsAsString.Add(meta);
                }
            }
            return (string.Join(" ", metaIdsAsString));
        }
    }

}
