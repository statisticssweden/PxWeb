using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PxWeb.Filters.Api2
{
    public class LangValidationFilter : Attribute, IResourceFilter
    {
        private readonly List<string> _languages;

        public LangValidationFilter(List<string> langs)
        {
            _languages = langs;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var lanValues = context.HttpContext.Request.Query["lang"].ToString();

            if (!string.IsNullOrEmpty(lanValues) && !_languages.Exists(x => x.ToString() == lanValues))
            {
                context.Result = new BadRequestObjectResult($"language {lanValues} is not a valid language");
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
