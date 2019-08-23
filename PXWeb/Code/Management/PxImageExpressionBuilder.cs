using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;
using System.Globalization;


namespace PXWeb.Management
{
    /// <summary>
    /// Class for getting localized string for the administration user interface
    /// </summary>
    [ExpressionPrefix("PxImage")]
    public class PxImageExpressionBuilder : ExpressionBuilder
    {
        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
        {
            CodeTypeReferenceExpression targetClass = new CodeTypeReferenceExpression(typeof(PxImageExpressionBuilder));
            const string targetMethod = "GetImagePath";
            CodeExpression methodParameter = new CodePrimitiveExpression(entry.Expression.Trim());
            return new CodeMethodInvokeExpression(targetClass, targetMethod, methodParameter);
        }

        public static string GetImagePath(string key)
        {
            return System.IO.Path.Combine(PXWeb.Settings.Current.General.Paths.ImagesPath, key);            
        }

    }

}
