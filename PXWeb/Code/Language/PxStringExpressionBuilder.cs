using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;
using System.Globalization;


namespace PXWeb.Language
{
    /// <summary>
    /// Class for getting localized string for the administration user interface
    /// </summary>
    [ExpressionPrefix("PxString")]
    public class PxStringExpressionBuilder : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression targetClass = new CodeTypeReferenceExpression(typeof(PxStringExpressionBuilder));
            const string targetMethod = "GetString";
            CodeExpression methodParameter = new CodePrimitiveExpression(entry.Expression.Trim());
            return new CodeMethodInvokeExpression(targetClass, targetMethod, methodParameter);
        }

        public static string GetString(string key)
        {
            //if (HttpContext.Current.Request.Params.AllKeys.Contains("px_language"))
            //{
            //    return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key,
            //        new CultureInfo(HttpContext.Current.Request.Params["px_language"]));
            //}
            var pxUrlObj = RouteInstance.PxUrlProvider.Create(null);

            string lang = pxUrlObj.Language; //PxUrl.GetParameter("px_language");

            if (!string.IsNullOrEmpty(lang))
            {
                return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key,
                    new CultureInfo(lang));
            }

            string cultureInfoStr = GetDefaultLanguage();

            if (HttpContext.Current.Session != null && HttpContext.Current.Session["adminlang"] as string != null)
            {
                cultureInfoStr = HttpContext.Current.Session["adminlang"] as string;
            }

            return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key,
             new CultureInfo(cultureInfoStr));

            //return PCAxis.Web.Core.Management.LocalizationManager.GetLocalizedString(key, 
            //    new CultureInfo(
            //        HttpContext.Current.Session["adminlang"] as string ?? GetDefaultLanguage()));
        }

        /// <summary>
        /// Get the default language
        /// </summary>
        /// <returns>Returns the default value. If no default language is specified english is returned</returns>
        private static string GetDefaultLanguage()
        {
            return Settings.Current.General.Language.DefaultLanguage ?? "en";
        }
    }
}
