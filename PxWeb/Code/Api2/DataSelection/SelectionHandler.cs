using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PxWeb.Code.Api2.DataSelection
{
    public class SelectionHandler : ISelectionHandler
    {
        public Selection[] GetSelection(PXModel model, VariablesSelection? variablesSelection)
        {
            if  (variablesSelection is not null && HasSelection(variablesSelection))
            {
                var selections = new List<Selection>();

                // TODO: Parse values for each variable in variablesSelection and add to selections

                return selections.ToArray();
            }
            else
            {
                return GetDefaultTable(model);
            }

        }

        public bool Verify(PXModel model, VariablesSelection? variablesSelection, out Problem? problem)
        {
            problem = null;

            // TODO: Verify that all variable and value codes defined in variablesSelection are found in model. If not return false and Problem. 
            // TODO: Verify that mandatory variables have at least one value selected
            //problem = NonExistentVariable();
            //return false;

            return true;
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

        private bool HasSelection(VariablesSelection selection)
        {
            if (selection.Selection.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Problem NonExistentVariable()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Non-existent variable";
            return p;
        }

        private Problem NonExistentValue()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Non-existent value";
            return p;
        }

        private Problem MissingSelection()
        {
            Problem p = new Problem();
            p.Type = "Parameter error";
            p.Status = 400;
            p.Title = "Missing selection for mandantory variable";
            return p;
        }

    }
}
