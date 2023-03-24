using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PxWeb.Api2.Server.Models;

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
                Problem p = new Problem();
                p.Type = "Parameter error";
                p.Title = "Unsupported language";
                p.Status = 400;
                p.Detail = $"Language {lanValues} is not supported";
                context.Result = new BadRequestObjectResult(p);
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
