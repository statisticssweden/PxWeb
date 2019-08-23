using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PCAxis.Paxiom;

namespace PCAxis.Sdmx.ExtensionMethods
{
    public static class SdmxStringExtensions
    {
        public static Regex WhiteExpression = new Regex("[^A-Za-z0-9-*_@$]");

        public static String CleanID(this String str)
        {
            return WhiteExpression.Replace(str, "");
        }

        public static string Agency(this PXMeta meta)
        {
            if (meta.Source != "")
            {
                return meta.Source;
            }
            //return ConverterSettings.Agancy;
            //TODO
            return "AGENCY_ID";
        }

        public static string ToSDMXTime(this Value value)
        {
            if (!value.Variable.HasTimeValue)
            {
                return "";
            }
            string time;
            switch (value.Variable.TimeScale)
            {
                case TimeScaleType.Annual:
                    time = value.TimeValue;
                    break;
                case TimeScaleType.Halfyear:
                    throw new PCAxis.Paxiom.PXException("Halfyear can not be converted to SDMX time format");
                case TimeScaleType.Monthly:
                    if (value.TimeValue.Contains("M"))
                    {
                        time = value.TimeValue.Replace('M', '-');
                    }
                    else
                    {
                        time = value.TimeValue.Insert(4, "-");
                    }
                    break;
                case TimeScaleType.Quartely:
                    time = value.TimeValue.Insert(4, "-Q");
                    break;
                case TimeScaleType.Weekly:
                    time = value.TimeValue.Substring(0, 4);
                    time = time + "-W" + int.Parse(value.TimeValue.Substring(5).Trim());
                    break;
                default:
                    time = "";
                    break;
            }
            return time;
        }

        public static org.sdmx.AnnotationType[] ToSDMXAnnotation(this Notes notes)
        {
            List<org.sdmx.AnnotationType> annotations = new List<org.sdmx.AnnotationType>();
            foreach (Note note in notes)
            {
                org.sdmx.AnnotationType annotation = new org.sdmx.AnnotationType();
                List<org.sdmx.TextType> annotationText = new List<org.sdmx.TextType>();
                org.sdmx.TextType text = new org.sdmx.TextType();
                text.Value = note.Text;
                text.lang = "en";
                annotationText.Add(text);
                annotation.AnnotationText = annotationText.ToArray();
                annotations.Add(annotation);
            }
            return annotations.ToArray();
        }
    }
}
