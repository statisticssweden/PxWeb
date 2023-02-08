using DocumentFormat.OpenXml.EMMA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PXWeb.Code.Management
{
    public class CnmmDatabaseRootHelper
    {
        public static string DatabaseRoot { get; set; }
        public static bool IsRooted { get; set; }

        public static bool Check(string Id)
        {
            if (IsRooted)
            {
                if (!string.IsNullOrEmpty(Id) && (Id.StartsWith(DatabaseRoot) || Id.StartsWith("START__" + DatabaseRoot)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static string GetId(string Id)
        {
            if (IsRooted)
            {
                if (string.IsNullOrEmpty(Id) || !Id.StartsWith(DatabaseRoot))
                {
                    return DatabaseRoot;
                }
            }
            return Id;
        }

    }
}