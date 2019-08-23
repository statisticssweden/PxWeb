using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using org.sdmx;
using PCAxis.Sdmx.ExtensionMethods;
using PCAxis.PlugIn;

namespace PCAxis.Sdmx
{
    public class SdmxDataSerializer : IPXModelStreamSerializer
    {

        public static GenericDataType createGenericData(PXModel model)
        {
            DataSetType ds = new DataSetType();

            ds.KeyFamilyRef = model.Meta.Matrix.CleanID();

            DataIndexer di = new DataIndexer(model.Meta);

            // Get all table level notes (this includes notes for variables)
            List<AnnotationType> dsAnnotations = new List<AnnotationType>();
            if (model.Meta.Notes != null)
            {
                dsAnnotations.AddRange(model.Meta.Notes.ToSDMXAnnotation());
            }
            foreach (Variable var in model.Meta.Stub)
            {
                if (var.Notes != null)
                {
                    dsAnnotations.AddRange(var.Notes.ToSDMXAnnotation());
                }
            }
            foreach (Variable var in model.Meta.Heading)
            {
                if (var.Notes != null)
                {
                    dsAnnotations.AddRange(var.Notes.ToSDMXAnnotation());
                }
            }

            if (dsAnnotations.Count > 0)
            {
                ds.Annotations = dsAnnotations.ToArray();
            }

            if (model.Meta.ContentVariable == null)
            {
                List<org.sdmx.ValueType> dsAtts = new List<org.sdmx.ValueType>();

                if (model.Meta.ContentInfo != null)
                {
                    // Unit of measure
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "UNIT_MEASURE";
                        att.value = model.Meta.ContentInfo.Units;
                        dsAtts.Add(att);
                    }
                    // Decimals
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "DECIMALS";
                        att.value = model.Meta.Decimals.ToString();
                        dsAtts.Add(att);
                    }

                    // Stock/flow/average indicator
                    if (model.Meta.ContentInfo.StockFa != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "SFA_INDICATOR";
                        att.value = model.Meta.ContentInfo.StockFa;
                        dsAtts.Add(att);
                    }

                    // Seasonal adjustement
                    if (model.Meta.ContentInfo.SeasAdj != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "SEAS_ADJ";
                        att.value = model.Meta.ContentInfo.SeasAdj;
                        dsAtts.Add(att);
                    }

                    // Daily adjustment
                    if (model.Meta.ContentInfo.DayAdj != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "DAY_ADJ";
                        att.value = model.Meta.ContentInfo.DayAdj;
                        dsAtts.Add(att);
                    }

                    // Base period
                    if (model.Meta.ContentInfo.Baseperiod != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "BASE_PER";
                        att.value = model.Meta.ContentInfo.Baseperiod;
                        dsAtts.Add(att);
                    }

                    // Reference period
                    if (model.Meta.ContentInfo.RefPeriod != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "REF_PERIOD";
                        att.value = model.Meta.ContentInfo.RefPeriod;
                        dsAtts.Add(att);
                    }

                    // Current / fixed prices
                    if (model.Meta.ContentInfo.CFPrices != null)
                    {
                        org.sdmx.ValueType att = new org.sdmx.ValueType();
                        att.concept = "PRICE_BASIS";
                        att.value = model.Meta.ContentInfo.CFPrices;
                        dsAtts.Add(att);
                    }

                }

                ds.Attributes = dsAtts.ToArray();
            }


            ds.Items = new Object[model.Data.MatrixRowCount];
            for (int i = 0; i < model.Data.MatrixRowCount; i++)
            {
                SeriesType series = new SeriesType();
                series.SeriesKey = new org.sdmx.ValueType[model.Meta.Stub.Count + 1];

                org.sdmx.ValueType key = new org.sdmx.ValueType();
                key.concept = "FREQ";

                switch (model.Meta.Heading[0].TimeScale)
                {
                    case TimeScaleType.Annual:
                        key.value = "A";
                        break;
                    case TimeScaleType.Halfyear:
                        key.value = "B";
                        break;
                    case TimeScaleType.Monthly:
                        key.value = "M";
                        break;
                    case TimeScaleType.Quartely:
                        key.value = "Q";
                        break;
                    case TimeScaleType.Weekly:
                        key.value = "W";
                        break;
                    default:
                        //TODO
                        break;
                }
                series.SeriesKey[0] = key;
                di.SetContext(i, 0);
                // Create annotations based on value notes (not variable notes)
                List<AnnotationType> serAnnotations = new List<AnnotationType>();
                for (int j = 0; j < model.Meta.Stub.Count; j++)
                {

                    key = new org.sdmx.ValueType();
                    key.concept = model.Meta.Stub[j].Name.CleanID();
                    key.value = model.Meta.Stub[j].Values[di.StubIndecies[j]].Code.CleanID();
                    series.SeriesKey[j + 1] = key;
                    if (model.Meta.Stub[j].Values[di.StubIndecies[j]].Notes != null)
                    {
                        serAnnotations.AddRange(model.Meta.Stub[j].Values[di.StubIndecies[j]].Notes.ToSDMXAnnotation());
                    }
                }
                if (serAnnotations.Count > 0)
                {
                    series.Annotations = serAnnotations.ToArray();
                }

                series.Obs = new ObsType[model.Data.MatrixColumnCount];
                //Added code for reading the cellnotes
                DataFormatter formatter = new DataFormatter(model);
                for (int j = 0; j < model.Data.MatrixColumnCount; j++)
                {
                    string notes = null;
                    ObsType obs = new ObsType();

                    // Set observation time
                    obs.Time = model.Meta.Heading[0].Values[j].ToSDMXTime();

                    Boolean missing = PXConstant.ProtectedNullValues.Contains(model.Data.ReadElement(i, j)) || PXConstant.ProtectedValues.Contains(model.Data.ReadElement(i, j));
                    //Create observation status attribute
                    org.sdmx.ValueType status = new org.sdmx.ValueType();
                    status.concept = "OBS_STATUS";

                    obs.Attributes = new org.sdmx.ValueType[1];
                    obs.Attributes[0] = status;

                    // Set observation value and status code
                    if (!missing)
                    {
                        obs.ObsValue = new ObsValueType();
                        obs.ObsValue.value = model.Data.ReadElement(i, j);
                        obs.ObsValue.valueSpecified = true;
                        status.value = "A";
                    }
                    else
                    {
                        status.value = "M";
                    }


                    // Cell notes
                    formatter.ReadElement(i, j, ref notes);

                    if (notes != null && notes.Length != 0)
                    {
                        AnnotationType annotation = new AnnotationType();
                        List<TextType> annotationText = new List<TextType>();
                        TextType text = new TextType();
                        text.lang = "en";
                        text.Value = notes;
                        annotationText.Add(text);
                        annotation.AnnotationText = annotationText.ToArray();
                        obs.Annotations = new AnnotationType[1];
                        obs.Annotations[0] = annotation;
                    }


                    series.Obs[j] = obs;
                }

                if (model.Meta.ContentVariable != null)
                {
                    List<org.sdmx.ValueType> serAtts = new List<org.sdmx.ValueType>();

                    if (model.Meta.ContentInfo != null)
                    {
                        // Unit of measure
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "UNIT_MEASURE";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.Units;
                            serAtts.Add(att);
                        }
                        // Stock/flow/average indicator
                        if (model.Meta.ContentInfo.StockFa != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "SFA_INDICATOR";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.StockFa;
                            serAtts.Add(att);
                        }

                        // Seasonal adjustement
                        if (model.Meta.ContentInfo.SeasAdj != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "SEAS_ADJ";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.SeasAdj;
                            serAtts.Add(att);
                        }

                        // Daily adjustment
                        if (model.Meta.ContentInfo.DayAdj != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "DAY_ADJ";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.DayAdj;
                            serAtts.Add(att);
                        }

                        // Base period
                        if (model.Meta.ContentInfo.Baseperiod != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "BASE_PER";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.Baseperiod;
                            serAtts.Add(att);
                        }

                        // Reference period
                        if (model.Meta.ContentInfo.RefPeriod != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "REF_PERIOD";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.RefPeriod;
                            serAtts.Add(att);
                        }

                        // Current / fixed prices
                        if (model.Meta.ContentInfo.CFPrices != null)
                        {
                            org.sdmx.ValueType att = new org.sdmx.ValueType();
                            att.concept = "PRICE_BASIS";
                            int cIndex = model.Meta.Stub.GetIndexByCode(model.Meta.ContentVariable.Code);
                            att.value = model.Meta.ContentVariable.Values[di.StubIndecies[cIndex]].ContentInfo.CFPrices;
                            serAtts.Add(att);
                        }

                    }
                    series.Attributes = serAtts.ToArray();
                }

                ds.Items[i] = series;
            }

            GenericDataType message = new GenericDataType();

            message.DataSet = ds;
            message.Header = createHeader(model, true);

            return message;
        }


        /// <summary>
        /// TODO GEMENSAM mellan de båda klasserna
        /// </summary>
        /// <param name="model"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static HeaderType createHeader(PXModel model, Boolean data)
        {
            HeaderType Header = new HeaderType();
            TextType Text = new TextType();

            String id = model.Meta.Matrix.CleanID();
            id += data ? "_DATA" : "_STRUCTURE";
            Header.ID = id;

            //Set creation date
            string sPrepared = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            Header.Prepared = sPrepared;
            Header.Test = false;

            //Create sender
            PartyType Sender = new PartyType();
            //Get name
            if (string.IsNullOrEmpty(model.Meta.Source))
            {

                Text.Value = "PX to SDMX-ML Converter application";
            }
            else
            {
                Text.Value = model.Meta.Source;
            }
            if (string.IsNullOrEmpty(model.Meta.Language))
            {
                Text.lang = "en";
            }
            else
            {
                Text.lang = model.Meta.Language.ToLower();
            }

            Sender.Name = new TextType[1];

            Sender.Name[0] = Text;
            Sender.id = "TEST";
            Header.Sender = new PartyType[1];
            Header.Sender[0] = Sender;
            //Set Data Set ID
            Header.DataSetID = model.Meta.Matrix.CleanID();
            //Set Key Fam ID
            Header.KeyFamilyRef = "N/A";

            return Header;
        }

        #region IPXModelStreamSerializer Members

        public void Serialize(PXModel model, System.IO.Stream stream)
        {
            PXModel m = Sdmx.SdmxStructureSerializer.RearrangeVariables(model);

            GenericDataType data = createGenericData(m);

            //TODO encoding
            //System.Text.Encoding encoding;
            //encoding = EncodingUtil.GetEncoding(model.Meta.CodePage);

            System.Xml.XmlWriterSettings xwSettings = new System.Xml.XmlWriterSettings();
            xwSettings.Indent = true;
            System.Xml.XmlWriter xmlWriter;
            xmlWriter = System.Xml.XmlTextWriter.Create(stream, xwSettings);
            XmlSerializer dataSer = new XmlSerializer(typeof(GenericDataType));
            XmlSerializerNamespaces dataNS = new XmlSerializerNamespaces();
            dataNS.Add("", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message");
            dataNS.Add("g", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic");
            dataNS.Add("c", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common");
            dataSer.Serialize(xmlWriter, data, dataNS);
            xmlWriter.Flush();

        }

        public void Serialize(PXModel model, string path)
        {
            if (model == null) throw new ArgumentNullException("model");

            // Let the StreamWriter verify the path argument
            using (System.IO.Stream writer = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                Serialize(model, writer);
            }
        }

        #endregion

    }
}
