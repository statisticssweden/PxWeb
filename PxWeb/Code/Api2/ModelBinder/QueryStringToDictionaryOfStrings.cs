using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PxWeb.Code.Api2.ModelBinder
{
    public class QueryStringToDictionaryOfStrings : IModelBinder
    {


        Task IModelBinder.BindModelAsync(ModelBindingContext bindingContext)
        {

            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var modelName = bindingContext.ModelName;

            var result = new Dictionary<string, List<string>>();

            var keys = bindingContext.HttpContext.Request.Query.Keys.ToList().Where(x => x.StartsWith(modelName, StringComparison.InvariantCultureIgnoreCase)).ToList();

            foreach (var key in keys)
            {
                //strip of the variable name format values[variableName]
                //TODO check that the key starts with [ after the modelname and ends with ]
                if (key != null) { 
                    string variableName = key.Substring(modelName.Length + 1, key.Length - (modelName.Length + 2));
                    string? q = bindingContext.HttpContext.Request.Query[key];
                    if (q != null) { 
                        string[] values = q.Split(new[] { '\u002C' }, StringSplitOptions.RemoveEmptyEntries);
                        result.Add(variableName, values.ToList());
                    }
                }
            }


            bindingContext.Result = ModelBindingResult.Success(result);

            return Task.CompletedTask;
        }
    }
}

