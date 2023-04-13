using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PxWeb.Code.Api2.DataSelection
{
    public class SelectionHandler : ISelectionHandler
    {
        public Selection[] GetSelection(PXModel model, VariablesSelection wantedSelection)
        {
            if (HasSelection(wantedSelection))
            {
                var selections = new List<Selection>();
                return selections.ToArray();
            }
            else
            {
                return GetDefaultTable(model);
            }

        }

        public bool Verify(PXModel model, VariablesSelection wantedSelection)
        {
            // TODO: Verify that all variable and value codes defined in wantedSelection are found in model. If not return false. 
            return true;
        }

        private bool HasSelection(VariablesSelection selection)
        {
            if (selection.Selection == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Selection[] GetDefaultTable(PXModel model)
        {
            //TODO implement the correct algorithm

            var selections = new List<Selection>();

            foreach (var variable in model.Meta.Variables)
            {
                var selection = new Selection(variable.Code);
                //Takes the first 4 values for each variable if variable has less values it takes all of its values.
                var codes = variable.Values.Take(4).Select(value => value.Code).ToArray();
                selection.ValueCodes.AddRange(codes);
                selections.Add(selection);
            }

            return selections.ToArray();
        }
    }
}
