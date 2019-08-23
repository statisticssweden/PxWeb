using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;
using org.sdmx;
using PCAxis.Sdmx.ExtensionMethods;

namespace PCAxis.Sdmx
{
    public class SdmxStructureSerializer : IPXModelStreamSerializer 
    {

        #region IPXModelStreamSerializer Members

        public void Serialize(PXModel model, System.IO.Stream stream)
        {
            PXModel m = RearrangeVariables(model);

            StructureType structure = createStructure(m);
            
            //TODO encoding
            //System.Text.Encoding encoding;
            //encoding = EncodingUtil.GetEncoding(model.Meta.CodePage);

            System.Xml.XmlWriterSettings xwSettings = new System.Xml.XmlWriterSettings();
            xwSettings.Indent = true;

            System.Xml.XmlWriter xmlWriter = System.Xml.XmlTextWriter.Create(stream, xwSettings);

            XmlSerializer strSer = new XmlSerializer(typeof(StructureType));
            XmlSerializerNamespaces strNS = new XmlSerializerNamespaces();
            strNS.Add("", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message");
            strNS.Add("s", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure");
            strNS.Add("c", "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common");
            strSer.Serialize(xmlWriter, structure, strNS);
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

        #region "Bla bla"
        

        /// <summary>
        /// Checks all precondition for the model to determin if it can be serialized to a SDMX structure file.
        /// </summary>
        /// <param name="model">PXModel to check</param>
        /// <returns>true if it can be converted false otherwise</returns>
        internal static bool CanConvertModel(PXModel model)
        {
            foreach (var variable in model.Meta.Variables)
            {
                if (variable.HasTimeValue) return true;
            }
            return false;
        }

        /// <summary>
        /// Rearranges the variables making the creation of the SDMX file easier.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static PXModel RearrangeVariables(PXModel model) { 
            //Verify all preconditions are ok.
            if (!CanConvertModel(model)) throw new PXSerializationException();

            Pivot pivot = new Pivot();
            PivotDescription[] pDesc;
            pDesc = new PivotDescription[model.Meta.Variables.Count];


            for (int i = 0; i < model.Meta.Variables.Count; i++)
            {
                Variable var = model.Meta.Variables[i];

                if (var.HasTimeValue)
                {
                    pDesc[i] = new PivotDescription(var.Name, PlacementType.Heading);
                }
                else
                {
                    pDesc[i] = new PivotDescription(var.Name, PlacementType.Stub);
                }
            }

            model = pivot.Execute(model, pDesc);
            return model;
        }

        #endregion


        static StructureType createStructure(PXModel model)
        {
            // Create structure
            StructureType structure = new StructureType();

            // Create concepts container
            ConceptsType concepts = new ConceptsType();
            structure.Concepts = concepts;
            // Concept Schemes
            List<ConceptSchemeType> conceptSchemes = new List<ConceptSchemeType>();
            //Create Common Concept Scheme
            conceptSchemes.Add(createCommonConcepts());
            // Create instance concept scheme
            ConceptSchemeType conceptScheme = new ConceptSchemeType();
            conceptScheme.agencyID = model.Meta.Agency().CleanID();
            conceptScheme.id = "DEFAULT";
            conceptScheme.version = "1.0";
            List<TextType> name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = "Default Concept Scheme";
                name.Add(enName);

            }
            conceptScheme.Name = name.ToArray();
            List<ConceptType> schemeConcepts = new List<ConceptType>();
            // Create fixed concepts (not contained in CDC)
            // Stock/flow/average indicator
            {
                ConceptType concept = new ConceptType();
                concept.id = "SFA_INDICATOR";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Stock, flow, average indicator";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }
            // Seasonal adjustement
            {
                ConceptType concept = new ConceptType();
                concept.id = "SEAS_ADJ";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Seasonally adjustement";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }
            // Daily adjustement
            {
                ConceptType concept = new ConceptType();
                concept.id = "DAY_ADJ";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Daily adjustement";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }
            // Price basis
            {
                ConceptType concept = new ConceptType();
                concept.id = "PRICE_BASIS";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Price basis";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            // Create codelists container
            List<CodeListType> codeLists = new List<CodeListType>();
            //Create Common Code Lists
            codeLists.AddRange(createCommonCodes(model));

            // Create key family
            KeyFamilyType keyFamily = new KeyFamilyType();
            keyFamily.agencyID = model.Meta.Agency().CleanID();
            keyFamily.id = model.Meta.Matrix.CleanID();
            keyFamily.version = "1.0";
            name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = model.Meta.Matrix;
                name.Add(enName);
            }
            keyFamily.Name = name.ToArray();
            ComponentsType components = new ComponentsType();
            keyFamily.Components = components;
            // Create attributes
            List<AttributeType> attributes = new List<AttributeType>();
            if (model.Meta.ContentVariable == null)
            {
                //Any non-null key words will be data set level attributes

                //Unit of measure
                attributes.Add(createUnitsAttribute(false, model.Meta.Agency().CleanID()));

                if (model.Meta.ContentInfo != null)
                {
                    // Stock, flow, average
                    if (model.Meta.ContentInfo.StockFa != null)
                    {
                        attributes.Add(createStockFlowAttribute(false, model.Meta.Agency().CleanID()));
                    }

                    // Seasonal adjustement
                    if (model.Meta.ContentInfo.SeasAdj != null)
                    {
                        attributes.Add(createSeasAdjAttribute(false, model.Meta.Agency().CleanID()));
                    }

                    // Daily adjustment
                    if (model.Meta.ContentInfo.DayAdj != null)
                    {
                        attributes.Add(createDayAdjAttribute(false, model.Meta.Agency().CleanID()));
                    }

                    // Base period
                    if (model.Meta.ContentInfo.Baseperiod != null)
                    {
                        attributes.Add(createBasePeriodAttribute(false));
                    }

                    // Reference period
                    if (model.Meta.ContentInfo.RefPeriod != null)
                    {
                        attributes.Add(createReferencePeriodAttribute(false));
                    }

                    // Current / fixed prices
                    if (model.Meta.ContentInfo.CFPrices != null)
                    {
                        attributes.Add(createPriceBasisAttribute(false, model.Meta.Agency().CleanID()));
                    }
                }

            }
            else
            {
                //Any non-null key words will be series level attributes

                //Unit of measure
                attributes.Add(createUnitsAttribute(true, model.Meta.Agency().CleanID()));

                if (model.Meta.ContentInfo != null)
                {
                    // Stock, flow, average
                    if (model.Meta.ContentInfo.StockFa != null)
                    {
                        attributes.Add(createStockFlowAttribute(true, model.Meta.Agency().CleanID()));
                    }

                    // Seasonal adjustement
                    if (model.Meta.ContentInfo.SeasAdj != null)
                    {
                        attributes.Add(createSeasAdjAttribute(true, model.Meta.Agency().CleanID()));
                    }

                    // Daily adjustment
                    if (model.Meta.ContentInfo.DayAdj != null)
                    {
                        attributes.Add(createDayAdjAttribute(true, model.Meta.Agency().CleanID()));
                    }

                    // Base period
                    if (model.Meta.ContentInfo.Baseperiod != null)
                    {
                        attributes.Add(createBasePeriodAttribute(true));
                    }

                    // Reference period
                    if (model.Meta.ContentInfo.RefPeriod != null)
                    {
                        attributes.Add(createReferencePeriodAttribute(true));
                    }

                    // Current / fixed prices
                    if (model.Meta.ContentInfo.CFPrices != null)
                    {
                        attributes.Add(createPriceBasisAttribute(true, model.Meta.Agency().CleanID()));
                    }
                }
            }
            attributes.Add(createObsStatusAttribute());
            attributes.Add(createDecimalsAttribute());

            // Add attributes to key family components
            components.Attribute = attributes.ToArray();

            // Create dimensions
            List<DimensionType> dimensions = new List<DimensionType>();
            // Create frequency dimension
            DimensionType freq = new DimensionType();
            freq.conceptAgency = "SDMX";
            freq.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            freq.conceptRef = "FREQ";
            freq.conceptVersion = "1.0";
            freq.codelistAgency = "SDMX";
            freq.codelist = "CL_FREQ";
            freq.codelistVersion = "1.0";
            freq.isFrequencyDimension = true;
            dimensions.Add(freq);

            // Loop through variables to create concepts, codelists, and dimensions
            foreach (Variable var in model.Meta.Variables)
            {
                if (!isStandardConcept(var))
                {
                    // Create concept
                    schemeConcepts.Add(createConcept(var));
                    // Create codelist
                    codeLists.Add(createCodelist(var));
                    // Create dimension
                    dimensions.Add(createDimension(var, model.Meta.Agency().CleanID()));
                }
            }
            // Create time dimension
            TimeDimensionType time = new TimeDimensionType();
            time.conceptAgency = "SDMX";
            time.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            time.conceptRef = "TIME_PERIOD";
            time.conceptVersion = "1.0";
            TextFormatType timeFormat = new TextFormatType();
            timeFormat.textType = TextTypeType.ObservationalTimePeriod;
            time.TextFormat = timeFormat;
            components.TimeDimension = time;

            // Add dimensions to key family components
            components.Dimension = dimensions.ToArray();

            // Create primary measure
            PrimaryMeasureType obsValue = new PrimaryMeasureType();
            obsValue.conceptSchemeAgency = "SDMX";
            obsValue.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            obsValue.conceptRef = "OBS_VALUE";
            obsValue.conceptVersion = "1.0";
            TextFormatType obsFormat = new TextFormatType();
            obsFormat.textType = TextTypeType.Decimal;
            obsValue.TextFormat = obsFormat;
            components.PrimaryMeasure = obsValue;

            // Add key family to structure
            List<KeyFamilyType> keyFamilies = new List<KeyFamilyType>();
            keyFamilies.Add(keyFamily);
            structure.KeyFamilies = keyFamilies.ToArray();
            // Add concepts to scheme
            conceptScheme.Concept = schemeConcepts.ToArray();
            conceptSchemes.Add(conceptScheme);
            concepts.ConceptScheme = conceptSchemes.ToArray();
            // Add codelists to structure
            structure.CodeLists = codeLists.ToArray();

            structure.Header = createHeader(model, false);

            return structure;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static ConceptSchemeType createCommonConcepts()
        {
            ConceptSchemeType conceptScheme = new ConceptSchemeType();
            conceptScheme.agencyID = "SDMX";
            conceptScheme.id = "CROSS_DOMAIN_CONCEPTS";
            conceptScheme.version = "1.0";
            List<TextType> name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = "SDMX Cross Domain Concept Scheme";
                name.Add(enName);

            }
            conceptScheme.Name = name.ToArray();
            List<ConceptType> schemeConcepts = new List<ConceptType>();

            //BASE PERIOD
            {
                ConceptType concept = new ConceptType();
                concept.id = "BASE_PER";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Base Period";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //DECIMALS
            {
                ConceptType concept = new ConceptType();
                concept.id = "DECIMALS";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Decimals";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //FREQ
            {
                ConceptType concept = new ConceptType();
                concept.id = "FREQ";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Frequency";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //OBS_VALUE
            {
                ConceptType concept = new ConceptType();
                concept.id = "OBS_VALUE";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Observation";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //OBS_STATUS
            {
                ConceptType concept = new ConceptType();
                concept.id = "OBS_STATUS";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Observation Status";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //REF_PERIOD
            {
                ConceptType concept = new ConceptType();
                concept.id = "REF_PERIOD";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Reference Period";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            //TIME_PERIOD
            {
                ConceptType concept = new ConceptType();
                concept.id = "TIME_PERIOD";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Time Period";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }


            //UNIT_MEASURE
            {
                ConceptType concept = new ConceptType();
                concept.id = "UNIT_MEASURE";
                name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Unit of Measure";
                    name.Add(enName);

                }
                concept.Name = name.ToArray();
                schemeConcepts.Add(concept);
            }

            conceptScheme.Concept = schemeConcepts.ToArray();
            return conceptScheme;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        static List<CodeListType> createCommonCodes(PXModel model)
        {
            List<CodeListType> codeLists = new List<CodeListType>();
            // Frequency
            {
                CodeListType codeList = new CodeListType();
                codeList.id = "CL_FREQ";
                codeList.agencyID = "SDMX";
                codeList.version = "1.0";
                List<TextType> name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Code list for Frequency (FREQ)";
                    name.Add(enName);

                }
                codeList.Name = name.ToArray();
                List<CodeType> codes = new List<CodeType>();
                {
                    CodeType code = new CodeType();
                    code.value = "A";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Annual";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "B";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Daily - business week";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "D";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Daily";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "M";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Monthly";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "N";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Minutely";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "Q";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Quarterly";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "S";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Half Yearly, semester";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "W";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Weekly";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                codeList.Code = codes.ToArray();

                codeLists.Add(codeList);
            }
            // Observation status
            {
                CodeListType codeList = new CodeListType();
                codeList.id = "CL_OBS_STATUS";
                codeList.agencyID = "SDMX";
                codeList.version = "1.0";
                List<TextType> name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Observation status";
                    name.Add(enName);

                }
                codeList.Name = name.ToArray();
                List<CodeType> codes = new List<CodeType>();
                {
                    CodeType code = new CodeType();
                    code.value = "A";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Normal";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "B";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Break";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "E";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Estimated value";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "F";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Forecast value";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "I";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Imputed value (CCSA definition)";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "M";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Missing value";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "P";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Provisional value";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "S";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Strike";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                codeList.Code = codes.ToArray();

                codeLists.Add(codeList);
            }
            // Unit of measure
            {
                CodeListType codeList = new CodeListType();
                codeList.id = "CL_UNIT_MEASURE";
                codeList.agencyID = model.Meta.Agency().CleanID();
                codeList.version = "1.0";
                List<TextType> name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Unit of measure code list";
                    name.Add(enName);

                }
                codeList.Name = name.ToArray();
                List<CodeType> codes = new List<CodeType>();
                if (model.Meta.ContentVariable == null)
                {
                    CodeType code = new CodeType();
                    code.value = model.Meta.ContentInfo.Units.CleanID();
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = model.Meta.ContentInfo.Units;
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                else
                {
                    List<String> codeValues = new List<String>();
                    foreach (Value value in model.Meta.ContentVariable.Values)
                    {
                        String codeValue = value.ContentInfo.Units.CleanID();
                        if (!codeValues.Contains(codeValue))
                        {
                            CodeType code = new CodeType();
                            code.value = codeValue;
                            List<TextType> description = new List<TextType>();
                            {
                                TextType enDesc = new TextType();
                                enDesc.lang = "en";
                                enDesc.Value = value.ContentInfo.Units;
                                description.Add(enDesc);
                            }
                            code.Description = description.ToArray();
                            codes.Add(code);
                            codeValues.Add(codeValue);
                        }
                    }
                }
                codeList.Code = codes.ToArray();
                codeLists.Add(codeList);
            }

            // Stock/flow/average indicator
            {
                CodeListType codeList = new CodeListType();
                codeList.id = "CL_SFA_INDICATOR";
                codeList.agencyID = model.Meta.Agency().CleanID();
                codeList.version = "1.0";
                List<TextType> name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Stock, flow, average indicator code list";
                    name.Add(enName);

                }
                codeList.Name = name.ToArray();
                List<CodeType> codes = new List<CodeType>();
                {
                    CodeType code = new CodeType();
                    code.value = "A";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Average";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "F";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Flow";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "S";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Stock";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                codeList.Code = codes.ToArray();
                codeLists.Add(codeList);
            }

            // Price basis
            {
                CodeListType codeList = new CodeListType();
                codeList.id = "CL_PRICE_BASIS";
                codeList.agencyID = model.Meta.Agency().CleanID();
                codeList.version = "1.0";
                List<TextType> name = new List<TextType>();
                {
                    TextType enName = new TextType();
                    enName.lang = "en";
                    enName.Value = "Price basis code list";
                    name.Add(enName);

                }
                codeList.Name = name.ToArray();
                List<CodeType> codes = new List<CodeType>();
                {
                    CodeType code = new CodeType();
                    code.value = "C";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Current Price";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                {
                    CodeType code = new CodeType();
                    code.value = "F";
                    List<TextType> description = new List<TextType>();
                    {
                        TextType enDesc = new TextType();
                        enDesc.lang = "en";
                        enDesc.Value = "Fixed Price";
                        description.Add(enDesc);
                    }
                    code.Description = description.ToArray();
                    codes.Add(code);
                }
                codeList.Code = codes.ToArray();
                codeLists.Add(codeList);
            }

            return codeLists;
        }

        static AttributeType createUnitsAttribute(Boolean series, String agency)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = "SDMX";
            att.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            att.conceptRef = "UNIT_MEASURE";
            att.conceptVersion = "1.0";

            att.codelistAgency = agency;
            att.codelist = "CL_UNIT_MEASURE";
            att.codelistVersion = "1.0";

            att.assignmentStatus = AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createStockFlowAttribute(Boolean series, String agency)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = agency;
            att.conceptSchemeRef = "DEFAULT";
            att.conceptRef = "SFA_INDICATOR";
            att.conceptVersion = "1.0";

            att.codelistAgency = agency;
            att.codelist = "CL_SFA_INDICATOR";
            att.codelistVersion = "1.0";

            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createPriceBasisAttribute(Boolean series, String agency)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = agency;
            att.conceptSchemeRef = "DEFAULT";
            att.conceptRef = "PRICE_BASIS";
            att.conceptVersion = "1.0";

            att.codelistAgency = agency;
            att.codelist = "CL_PRICE_BASIS";
            att.codelistVersion = "1.0";

            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createSeasAdjAttribute(Boolean series, String agency)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = agency;
            att.conceptSchemeRef = "DEFAULT";
            att.conceptRef = "SEAS_ADJ";
            att.conceptVersion = "1.0";

            TextFormatType format = new TextFormatType();
            format.textType = TextTypeType.Boolean;
            att.TextFormat = format;


            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createDayAdjAttribute(Boolean series, String agency)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = agency;
            att.conceptSchemeRef = "DEFAULT";
            att.conceptRef = "DAY_ADJ";
            att.conceptVersion = "1.0";

            TextFormatType format = new TextFormatType();
            format.textType = TextTypeType.Boolean;
            att.TextFormat = format;


            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createBasePeriodAttribute(Boolean series)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = "SDMX";
            att.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            att.conceptRef = "BASE_PER";
            att.conceptVersion = "1.0";

            TextFormatType format = new TextFormatType();
            format.textType = TextTypeType.String;
            att.TextFormat = format;

            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createReferencePeriodAttribute(Boolean series)
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = "SDMX";
            att.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            att.conceptRef = "REF_PERIOD";
            att.conceptVersion = "1.0";

            TextFormatType format = new TextFormatType();
            format.textType = TextTypeType.String;
            att.TextFormat = format;

            att.assignmentStatus = series ? AssignmentStatusType.Conditional : AssignmentStatusType.Mandatory;
            att.attachmentLevel = series ? AttachmentLevelType.Series : AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createDecimalsAttribute()
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = "SDMX";
            att.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            att.conceptRef = "DECIMALS";
            att.conceptVersion = "1.0";

            TextFormatType format = new TextFormatType();
            format.textType = TextTypeType.Integer;
            att.TextFormat = format;

            att.assignmentStatus = AssignmentStatusType.Mandatory;
            att.attachmentLevel = AttachmentLevelType.DataSet;

            return att;
        }

        static AttributeType createObsStatusAttribute()
        {
            AttributeType att = new AttributeType();
            att.conceptSchemeAgency = "SDMX";
            att.conceptSchemeRef = "CROSS_DOMAIN_CONCEPTS";
            att.conceptRef = "OBS_STATUS";
            att.conceptVersion = "1.0";

            att.codelistAgency = "SDMX";
            att.codelist = "CL_OBS_STATUS";
            att.codelistVersion = "1.0";

            att.assignmentStatus = AssignmentStatusType.Mandatory;
            att.attachmentLevel = AttachmentLevelType.Observation;

            return att;
        }

        static Boolean isStandardConcept(Variable var)
        {
            return var.IsTime;
        }

        static ConceptType createConcept(Variable var)
        {
            ConceptType concept = new ConceptType();
            concept.id = var.Name.CleanID();
            List<TextType> name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = var.Name;
                name.Add(enName);

            }
            concept.Name = name.ToArray();
            return concept;
        }
        static CodeListType createCodelist(Variable var)
        {
            CodeListType codelist = new CodeListType();
            codelist.agencyID = var.Meta.Agency().CleanID();
            codelist.id = ("CL_" + var.Name).CleanID();
            codelist.version = "1.0";
            List<TextType> name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = var.Name + " Code List";
                name.Add(enName);

            }
            codelist.Name = name.ToArray();
            List<CodeType> codes = new List<CodeType>();
            foreach (Value val in var.Values)
            {
                codes.Add(createCode(val, var.Values.IsCodesFictional));
            }
            codelist.Code = codes.ToArray();
            return codelist;
        }

        static CodeType createCode(Value val, Boolean isFictional)
        {
            CodeType code = new CodeType();

            List<TextType> name = new List<TextType>();
            {
                TextType enName = new TextType();
                enName.lang = "en";
                enName.Value = val.Value;
                name.Add(enName);
            }
            code.Description = name.ToArray();
            if (isFictional)
            {
                code.value = val.Value.CleanID();
            }
            else
            {
                code.value = val.Code.CleanID();
            }
            return code;
        }

        static DimensionType createDimension(Variable var, String agency)
        {
            DimensionType dimension = new DimensionType();
            dimension.conceptRef = var.Name.CleanID();
            dimension.conceptSchemeAgency = agency;
            dimension.conceptSchemeRef = "DEFAULT";
            dimension.conceptVersion = "1.0";
            dimension.codelistAgency = agency;
            dimension.codelist = ("CL_" + var.Name).CleanID();
            dimension.codelistVersion = "1.0";
            return dimension;
        }

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

    }
}
