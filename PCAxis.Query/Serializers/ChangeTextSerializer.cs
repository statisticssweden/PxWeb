using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Query.Serializers
{
    public class ChangeTextSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is ChangeTextDescription))
            {
                //TODO throw exception
                return null;
            }
        
            var desc = obj as ChangeTextDescription;
            var step = new WorkStep();

            step.Id = Guid.NewGuid().ToString();

            step.Type = OperationConstants.CHANGE_TEXT;
            step.Params.Add("content", desc.Content);
            step.Params.Add("units", desc.Units);
            step.Params.Add("variables._count", desc.Variables.Count.ToString());
            for (int i = 0; i < desc.Variables.Count; i++)
            {
                step.Params.Add("variables." + i + ".code", desc.Variables[i].Key);
                step.Params.Add("variables." + i + ".name", desc.Variables[i].Value);
            }

            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.CHANGE_TEXT)
            {
                //TODO throw exception
                return null;
            }

            var desc = new ChangeTextDescription();
            desc.Content = step.Params["content"];
            desc.Units = step.Params["units"];

            int count;
            if (!int.TryParse(step.Params["variables._count"], out count))
            {
                //TODO throw Exception
                return null;
            }

            for (int i = 0; i < count; i++)
            {
                var kvp = new KeyValuePair<string, string>(step.Params["variables." + i + ".code"], step.Params["variables." + i + ".name"]);
                desc.Variables.Add(kvp);
            }

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new ChangeText();
        }
    }
}
