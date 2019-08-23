using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Extensions;

namespace PCAxis.Search
{
    /// <summary>
    /// Paxiom extensions
    /// </summary>
    public static class PaxiomHelperExtensions
    {
        public static string GetLastUpdated(this PXMeta meta)
        {
            DateTime lastUpdated = DateTime.MinValue;
            DateTime tryDate;

            if (meta.ContentVariable != null && meta.ContentVariable.Values.Count > 0)
            {
                foreach (Value value in meta.ContentVariable.Values)
                {
                    if (value.ContentInfo != null)
                    {
                        if (value.ContentInfo.LastUpdated != null)
                        {
                            if (value.ContentInfo.LastUpdated.IsPxDate())
                            {
                                tryDate = value.ContentInfo.LastUpdated.PxDateStringToDateTime();
                                if (tryDate > lastUpdated)
                                {
                                    lastUpdated = tryDate;
                                }
                            }
                        }
                    }
                }
            }
            else if (meta.ContentInfo != null)
            {
                if (!string.IsNullOrEmpty(meta.ContentInfo.LastUpdated))
                {
                    if (meta.ContentInfo.LastUpdated.IsPxDate())
                    {
                        tryDate = meta.ContentInfo.LastUpdated.PxDateStringToDateTime();
                        if (tryDate > lastUpdated)
                        {
                            lastUpdated = tryDate;
                        }
                    }
                }
            }

            return lastUpdated.ToString(PCAxis.Paxiom.PXConstant.PXDATEFORMAT);
        }

        public static string GetTimeValues(this PXMeta meta)
        {
            for (int i = 0; i < meta.Variables.Count; i++)
            {
                if (meta.Variables[i].IsTime)
                {
                    return string.Join(" ", (from v in meta.Variables[i].Values select v.Value).ToArray());
                }
            }
            return "";
        }

        public static string GetAllValues(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (!var.IsTime)
                {
                    foreach (var val in var.Values)
                    {
                        builder.Append(val.Value);
                        builder.Append(" ");
                    }
                }
            }
            return builder.ToString();
        }

        public static string GetAllCodes(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (!var.IsTime)
                {
                    foreach (var val in var.Values)
                    {
                        if (!var.Values.IsCodesFictional)
                        {
                            builder.Append(val.Code);
                            builder.Append(" ");
                        }
                    }
                }
            }
            return builder.ToString();
        }

        public static string GetAllGroupings(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (var.HasGroupings())
                {
                    foreach (GroupingInfo gi in var.Groupings)
                    {
                        builder.Append(gi.Name);
                        builder.Append(" ");
                    }
                }
            }
            return builder.ToString();
        }

        public static string GetAllGroupingCodes(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (var.HasGroupings())
                {
                    foreach (GroupingInfo gi in var.Groupings)
                    {
                        builder.Append(gi.ID);
                        builder.Append(" ");
                    }
                }
            }
            return builder.ToString();
        }
        public static string GetAllValuesets(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (var.HasValuesets())
                {
                    foreach (ValueSetInfo vi in var.ValueSets)
                    {
                        builder.Append(vi.Name);
                        builder.Append(" ");
                    }
                }
            }
            return builder.ToString();
        }
        public static string GetAllValuesetCodes(this PXMeta meta)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var var in meta.Variables)
            {
                if (var.HasValuesets())
                {
                    foreach (ValueSetInfo vi in var.ValueSets)
                    {
                        builder.Append(vi.ID);
                        builder.Append(" ");
                    }
                }
            }
            return builder.ToString();
        }
    }
}
