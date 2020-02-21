
namespace PCAxis.Serializers.JsonStat
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PCAxis.Paxiom;
    using log4net;
    using PCAxis.Paxiom.Extensions;
    using PCAxis.Metadata;
    using System.Collections;
	using Newtonsoft.Json;

	public class JsonStatSerializer : PCAxis.Paxiom.IPXModelStreamSerializer
    {
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(JsonStatSerializer));

        // A map between the numerical status-symbols in the array and the actual representation of it
        private Dictionary<double, string> dataSymbolMap = new Dictionary<double, string>();
        private Dictionary<string, bool> geoVariableMap = new Dictionary<string, bool>();
        private PXMeta meta;
        private MetaLinkManager metaLinkManager = new MetaLinkManager();

        // Reserved fields in JSON-stat (in no particular order)
        private const string TIME = "time";
        private const string METRIC = "metric";
        private const string UNIT = "unit";
        private const string BASE = "base";
        private const string ROLE = "role";
        private const string GEO = "geo";
        private const string CATEGORY = "category";
        private const string ID = "id";
        private const string SIZE = "size";
        private const string INDEX = "index";
        private const string LABEL = "label";
        private const string DECIMALS = "decimals";
        private const string LINK = "link";
        private const string DESCRIBEDBY = "describedby";
        private const string EXTENSION = "extension";

		//Field in JSON-Stat, used for PX extention 
		private const string PX = "px";

		private string datasetTitle;

        /// <summary>
        /// Set an alternative title to the dataset instead of the generated one
        /// </summary>
        public string DatasetTitle
        {
            get { return datasetTitle; }
            set { datasetTitle = value; }
        }

        private string[] geoVariableCodes;

        /// <summary>
        /// String array of variable codes that are considered as GEO-variables. Use only if you know certain variables
        /// are mappable but don't have related maps. Ex. ['Grunnkretser','Land']
        /// </summary>
        public string[] GeoVariableCodes
        {
            get { return geoVariableCodes; }
            set
            {
                geoVariableCodes = value;
                geoVariableMap.Clear();
                foreach (var v in value)
                {
                    geoVariableMap.Add(v, true);
                }
            }
        }

        private void BuildDataSymbolMap()
        {
            // Would have been handy to actually have this as a map in PAxiom.
            dataSymbolMap.Add(PXConstant.DATASYMBOL_1, string.IsNullOrEmpty(meta.DataSymbol1) ? PXConstant.DATASYMBOL_1_STRING : meta.DataSymbol1);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_2, string.IsNullOrEmpty(meta.DataSymbol2) ? PXConstant.DATASYMBOL_2_STRING : meta.DataSymbol2);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_3, string.IsNullOrEmpty(meta.DataSymbol3) ? PXConstant.DATASYMBOL_3_STRING : meta.DataSymbol3);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_4, string.IsNullOrEmpty(meta.DataSymbol4) ? PXConstant.DATASYMBOL_4_STRING : meta.DataSymbol4);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_5, string.IsNullOrEmpty(meta.DataSymbol5) ? PXConstant.DATASYMBOL_5_STRING : meta.DataSymbol5);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_6, string.IsNullOrEmpty(meta.DataSymbol6) ? PXConstant.DATASYMBOL_6_STRING : meta.DataSymbol6);
            dataSymbolMap.Add(PXConstant.DATASYMBOL_7, string.IsNullOrEmpty(meta.DataSymbolSum) ? PXConstant.DATASYMBOL_7_STRING : meta.DataSymbolSum); //Strange code due to lagacy which has been addressed but not fixed
            dataSymbolMap.Add(PXConstant.DATASYMBOL_NIL, string.IsNullOrEmpty(meta.DataSymbolNIL) ? PXConstant.DATASYMBOL_NIL_STRING : meta.DataSymbolNIL);
        }

        public void Serialize(PXModel model, System.IO.Stream stream)
        {
            var result = GenerateJsonData(model);

            byte[] jsonData = System.Text.Encoding.UTF8.GetBytes(result);
            stream.Write(jsonData, 0, jsonData.Length);
        }

        public void Serialize(PXModel model, string path)
        {
            var result = GenerateJsonData(model);
            var encoding = new UTF8Encoding();

            System.IO.File.WriteAllText(path, result, encoding);
        }

        private string GenerateJsonData(PXModel model)
        {
            meta = model.Meta;

            _logger.Info("Trying to parse .CreationDate from Paxiom => " + meta.CreationDate);

            BuildDataSymbolMap();

            var jsonResult = new JsonStat.Model.JsonStat();
            var dataset = new JsonStat.Model.JsonStatDatasetBase(model.Data.MatrixSize);
            var formatter = new DataFormatter(model);

            dataset.source = meta.Source;
            dataset.updated = meta.CreationDate.PxDateStringToDateTime().ToString();
            dataset.dimension = new Dictionary<string, object>();

			//Extension, PX 
			if (meta.InfoFile != null || meta.TableID != null || meta.Decimals != -1)
			{
				dataset.extension = new Dictionary<string, object>();
				var px = new JsonStat.Model.JsonStatPx();

				px.infofile = meta.InfoFile;
				px.tableid = meta.TableID;
				//If not Showdecimal has value use Decimal
				var decimals = meta.ShowDecimals < 0 ? meta.Decimals : meta.ShowDecimals;
				px.decimals = decimals;

				dataset.extension.Add(PX, px);
			}

			if (DatasetTitle != null)
            {
                dataset.label = DatasetTitle;
            }
            else
            {
                dataset.label = meta.Title;
            }

            var roles = new Model.JsonStatDimension();

            var id = new string[meta.Variables.Count];
            var size = new int[meta.Variables.Count];
            var variableIndex = 0;

            #region Variables
            foreach (var variable in meta.Variables)
            {
                var variableName = variable.Code;

                var category = new Dictionary<string, object>();
                var index = new Dictionary<string, int>();
                var label = new Dictionary<string, string>();
                var variableEntry = new Dictionary<string, object>();
                var unit = new Dictionary<string, object>();
                var link = new Dictionary<string, object>();

                if (variable.IsContentVariable)
                {
                    // The reason roles are an array, is that it's possible to have several time-variables (in theory)
                    roles.Add(METRIC, variableName);
                }
                else if (variable.IsTime)
                {
                    roles.Add(TIME, variableName);
                }

                // Adding regional variables to the geo-field.
                if (variable.Map != null || geoVariableMap.ContainsKey(variableName))
                {
                    roles.Add(GEO, variableName);
                }

                id[variableIndex] = variableName;
                size[variableIndex] = variable.Values.Count;
                variableIndex++;

                var variableValueIndex = 0;

                foreach (var value in variable.Values)
                {
                    index.Add(value.Code, variableValueIndex);
                    label.Add(value.Code, value.Value);

                    if (variable.IsContentVariable)
                    {
                        var unitContent = new Dictionary<string, object>();
                        unitContent.Add(BASE, value.ContentInfo.Units);
                        //If not Showdecimal has value use Decimal
                        var currentDecimal = meta.ShowDecimals < 0 ? meta.Decimals : meta.ShowDecimals;
                        var decimals = (value.HasPrecision()) ? value.Precision : currentDecimal;
                        unitContent.Add(DECIMALS, decimals);
                        unit.Add(value.Code, unitContent);
                    }
                    variableValueIndex++;
                }

                category.Add(INDEX, index);
                category.Add(LABEL, label);

                if (variable.IsContentVariable)
                {
                    category.Add(UNIT, unit);
                }

                variableEntry.Add(LABEL, variable.Name);
                variableEntry.Add(CATEGORY, category);
                var extensions = GetAllSerializedMetaIdsForVariable(variable);
                if (extensions.Count > 0)
                {
                    link.Add(DESCRIBEDBY, new List<object> { extensions });
                    variableEntry.Add(LINK, link);
                }
                dataset.dimension.Add(variableName, variableEntry);

            }
            #endregion

            dataset.dimension.Add(ID, id);
            dataset.dimension.Add(SIZE, size);
            dataset.dimension.Add(ROLE, roles);

            // All data is in a single array in JSON-stat.
            #region Data

            var haveObsStatus = false;
            var observationStatus = new Dictionary<string, string>();
            var bufferIndex = 0;
            var buffer = new double[model.Data.MatrixColumnCount];
            string note = string.Empty;
            string dataNote = string.Empty;
            var numberFormatInfo = new System.Globalization.NumberFormatInfo();

            for (int i = 0; i < model.Data.MatrixRowCount; i++)
            {
                model.Data.ReadLine(i, buffer);
                for (int j = 0; j < model.Data.MatrixColumnCount; j++)
                {
                    string symbol = null;

                    if (dataSymbolMap.TryGetValue(buffer[j], out symbol))
                    {
                        observationStatus.Add(bufferIndex.ToString(), symbol);
                        haveObsStatus = true;

                        dataset.value[bufferIndex] = null;
                    }
                    else
                    {
                        dataset.value[bufferIndex] = Convert.ToDouble(formatter.ReadElement(i, j, ref note, ref dataNote, ref numberFormatInfo), numberFormatInfo);

                        if (!string.IsNullOrEmpty(dataNote))
                        {
                            observationStatus.Add(bufferIndex.ToString(), dataNote);
                            haveObsStatus = true;
                        }
                    }

                    bufferIndex++;
                }
            }

            if (!haveObsStatus)
            {
                jsonResult.dataset = dataset;
            }
            else
            {
                var datasetEx = new Model.JsonStatDatasetExtended(model.Data.MatrixSize);

                // Copy values from the baseobject to the extended object. And add observation status.
                datasetEx.dimension = dataset.dimension;
                datasetEx.label = dataset.label;
                datasetEx.source = dataset.source;
                datasetEx.updated = dataset.updated;
                datasetEx.value = dataset.value;
                datasetEx.status = observationStatus;

                jsonResult.dataset = datasetEx;
            }

            #endregion

  		    // override converter to stop adding ".0" after interger values.
			string result = JsonConvert.SerializeObject(jsonResult, new DecimalJsonConverter());


			return result;
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
