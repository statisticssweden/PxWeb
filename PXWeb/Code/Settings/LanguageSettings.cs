using System.Xml;

namespace PXWeb
{
    /// <summary>
    /// Internal class for reading and writing the General.Languages.Language settings
    /// </summary>
    internal class LanguageSettings : ILanguageSettings
    {
        #region "public methods"

        /// <summary>
        /// Constructor
        /// </summary>
        public LanguageSettings()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="languageNode">XML-node for General.Languages´.Language settings</param>
        public LanguageSettings(XmlNode languageNode)
        {
            string xpath;
            PCAxis.Paxiom.Settings.LocaleSettings defaultValues;

            // Load the Language settings 

            // Name
            xpath = "./name";
            Name = SettingsHelper.GetSettingValue(xpath, languageNode, "");

            if (Name.Length > 0)
            {
                // Get default values for this language
                defaultValues = PCAxis.Paxiom.Settings.GetLocale(Name);

                // Default language
                xpath = "./default";
                DefaultLanguage = SettingsHelper.GetSettingValue(xpath, languageNode, false);

                // Decimal separator
                xpath = "./decimalSeparator";
                DecimalSeparator = SettingsHelper.GetSettingValue(xpath, languageNode, defaultValues.DecimalSeparator);
                DecimalSeparator = DecodeDecimalSeparator(DecimalSeparator);

                // Thousand separator
                xpath = "./thousandSeparator";
                ThousandSeparator = SettingsHelper.GetSettingValue(xpath, languageNode, defaultValues.ThousandSeparator);
                ThousandSeparator = DecodeThousandSeparator(ThousandSeparator);

                // Date format
                xpath = "./dateFormat";
                DateFormat = SettingsHelper.GetSettingValue(xpath, languageNode, defaultValues.DateFormat);
            }
        }

        /// <summary>
        /// Save General.Languages.Language settings to the settings file
        /// </summary>
        /// <param name="siteNode">XML-node for General.Languages.Language settings</param>
        public void Save(XmlNode languageNode)
        {
            XmlNode node;

            if (languageNode != null)
            {
                //Name
                node = languageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "name", "");
                node.InnerText = Name;
                languageNode.AppendChild(node);

                //Default
                node = languageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "default", "");
                node.InnerText = DefaultLanguage.ToString();
                languageNode.AppendChild(node);

                // Decimal separator
                node = languageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "decimalSeparator", "");
                node.InnerText = EncodeDecimalSeparator(DecimalSeparator);
                languageNode.AppendChild(node);

                // Thousand separator
                node = languageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "thousandSeparator", "");
                node.InnerText = EncodeThousandSeparator(ThousandSeparator);
                languageNode.AppendChild(node);

                // Date format
                node = languageNode.OwnerDocument.CreateNode(XmlNodeType.Element, "dateFormat", "");
                node.InnerText = DateFormat;
                languageNode.AppendChild(node);
            }
        }

        #endregion

        #region "public static methods"

        /// <summary>
        /// Get uncoded decimal separator
        /// </summary>
        /// <param name="decimalSeparator">Decimal separator</param>
        /// <returns>Decimal separator character</returns>
        public static string DecodeDecimalSeparator(string decimalSeparator)
        {
            switch (decimalSeparator)
            {
                case "Point":
                    return ".";
                case "Comma":
                    return ",";
                default:
                    return decimalSeparator;
            }
        }

        /// <summary>
        /// Get uncoded thousand separator
        /// </summary>
        /// <param name="thousandSeparator">Thousand separator</param>
        /// <returns>Thousand separator character</returns>
        public static string DecodeThousandSeparator(string thousandSeparator)
        {
            switch (thousandSeparator)
            {
                case "Point":
                    return ".";
                case "Comma":
                    return ",";
                case "Space":
                    return " ";
                case "None":
                    return "";
                default:
                    return thousandSeparator;
            }
        }

        /// <summary>
        /// Encode decimal separator for storage in config file
        /// </summary>
        /// <param name="decimalSeparator">decimal separator</param>
        /// <returns>Encoded decimal separator</returns>
        public static string EncodeDecimalSeparator(string decimalSeparator)
        {
            switch (decimalSeparator)
            {
                case ".":
                    return "Point";
                case ",":
                    return "Comma";
                default:
                    return decimalSeparator;
            }
        }

        /// <summary>
        /// Encode thousand separator for storage in config file
        /// </summary>
        /// <param name="thousandSeparator">thousand separator</param>
        /// <returns>Encoded thousand separator</returns>
        public static string EncodeThousandSeparator(string thousandSeparator)
        {
            switch (thousandSeparator)
            {
                case ".":
                    return "Point";
                case ",":
                    return "Comma";
                default:
                    if (thousandSeparator.Length == 0)
                    {
                        return "None";
                    }
                    else if (char.IsWhiteSpace(thousandSeparator[0]))
                    {
                        return "Space";
                    }
                    else
                    {
                        return "None";
                    }
            }
        }

        #endregion

        #region ILanguageSettings Members

        /// <summary>
        /// Name (code) for the language
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If it is the default language
        /// </summary>
        public bool DefaultLanguage { get; set; }

        /// <summary>
        /// String used as decimal separator
        /// </summary>
        public string DecimalSeparator { get; set; }

        /// <summary>
        /// String used as thousand separator
        /// </summary>
        public string ThousandSeparator { get; set; }

        /// <summary>
        /// Date format
        /// </summary>
        public string DateFormat { get; set; }
        #endregion


    }

}
