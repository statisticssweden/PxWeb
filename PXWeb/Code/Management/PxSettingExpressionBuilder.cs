using System;
using System.CodeDom;
using System.Web.Compilation;
using System.Web.UI;


namespace PXWeb.Management
{
    [ExpressionPrefix("PxSetting")]
    class PxSettingExpressionBuilder : ExpressionBuilder
    {
        public override object ParseExpression(string expression,
         Type propertyType, ExpressionBuilderContext context)
        {
            return expression;
        }

        public override CodeExpression GetCodeExpression(BoundPropertyEntry entry,
         object parsedData, ExpressionBuilderContext context)
        {
            return new CodeSnippetExpression(parsedData.ToString());
        }
    }
}
