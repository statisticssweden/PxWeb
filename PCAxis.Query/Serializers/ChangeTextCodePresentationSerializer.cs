using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAxis.Paxiom;
using PCAxis.Paxiom.Operations;

namespace PCAxis.Query.Serializers
{
    /// <summary>
    /// Class for serializing the ChangeTextCodePresentation operation
    /// </summary>
    public class ChangeTextCodePresentationSerializer : IOperationSerializer
    {
        public WorkStep Serialize(object obj)
        {
            if (!(obj is ChangeTextCodePresentationDescription))
            {
                //TODO throw exception
                return null;
            }

            var desc = obj as ChangeTextCodePresentationDescription;
            var step = new WorkStep();

            step.Id = Guid.NewGuid().ToString();
            step.Type = OperationConstants.CHANGE_TEXT_CODE_PRESENTATION;

            step.Params.Add("variables._count", desc.PresentationDictionary.Count.ToString());

            int i = 0;
            foreach (KeyValuePair<String, HeaderPresentationType> kvp in desc.PresentationDictionary)
            {
                step.Params.Add("variables." + i + ".code", kvp.Key);
                step.Params.Add("variables." + i + ".presentationType", kvp.Value.ToString());
                i++;
            }
            return step;
        }

        public object Deserialize(WorkStep step)
        {
            if (step.Type != OperationConstants.CHANGE_TEXT_CODE_PRESENTATION)
            {
                //TODO throw exception
                return null;
            }

            var desc = new ChangeTextCodePresentationDescription();

            int count;
            if (!int.TryParse(step.Params["variables._count"], out count))
            {
                //TODO throw Exception
                return null;
            }

            for (int i = 0; i < count; i++)
            {
                string code = step.Params["variables." + i + ".code"];
                HeaderPresentationType presType = (HeaderPresentationType)Enum.Parse(typeof(HeaderPresentationType), step.Params["variables." + i + ".presentationType"], true);
                desc.PresentationDictionary.Add(code, presType);
            }

            return desc;
        }

        public Paxiom.IPXOperation CreateOperation()
        {
            return new ChangeTextCodePresentation();
        }
    }
}
