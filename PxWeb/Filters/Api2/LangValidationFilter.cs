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
                //context.Result = new CustomResult("No such language" );
                context.Result = new NotFoundResult();
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public class CustomResult : NotFoundResult
        {
            private readonly string Reason;

            public CustomResult(string reason) : base()
            {
                Reason = reason;
            }

            public override void ExecuteResult(ActionContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }
                //context.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = Reason;
                context.HttpContext.Response.StatusCode = StatusCode;
                
                var bytes = Encoding.UTF8.GetBytes(Reason);
                context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);

            }
        }

    }
}
