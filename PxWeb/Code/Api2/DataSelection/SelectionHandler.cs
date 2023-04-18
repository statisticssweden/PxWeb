using Lucene.Net.Util;
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

        //private Selection[] GetDefaultTable(PXModel model)
        //{
        //    //TODO implement the correct algorithm

        //    var selections = new List<Selection>();

        //    foreach (var variable in model.Meta.Variables)
        //    {
        //        var selection = new Selection(variable.Code);
        //        //Takes the first 4 values for each variable if variable has less values it takes all of its values.
        //        var codes = variable.Values.Take(4).Select(value => value.Code).ToArray();
        //        selection.ValueCodes.AddRange(codes);
        //        selections.Add(selection);
        //    }

        //    return selections.ToArray();
        //}

        private Selection[] GetDefaultTable(PXModel model)
        {
            var selections = new List<Selection>();

            // Only select from variables that cannot be eliminated
            var variablesWithSelection = model.Meta.Variables.FindAll(v => v.Elimination == false);

            // Find the time variable
            var timeVar = variablesWithSelection.Find(v => v.IsTime == true);

            // Find the contents variable
            var contentVar = variablesWithSelection.Find(v => v.IsContentVariable == true);

            // 2 variables - Time and Content
            if (variablesWithSelection.Count == 2 && timeVar != null && contentVar != null)
            {
                foreach (var variable in model.Meta.Variables)
                {
                    var selection = new Selection(variable.Code);

                    if (variable.IsContentVariable)
                    {
                        // Content - Takes all values
                        var codes = contentVar.Values.Take(contentVar.Values.Count).Select(value => value.Code).ToArray();
                        selection.ValueCodes.AddRange(codes);
                    }
                    else if (variable.IsTime)
                    {
                        // Time - Take the 12 last values
                        var lstCodes = timeVar.Values.TakeLast(12).Select(value => value.Code).ToList();
                        lstCodes.Sort((a, b) => b.CompareTo(a)); // Descending sort
                        var codes = lstCodes.ToArray();
                        selection.ValueCodes.AddRange(codes);
                    }
                    else
                    {
                        // Select nothing for the other variables
                    }

                    selections.Add(selection);
                }

                //// Content - Takes all values
                //var selection = new Selection(contentVar.Code);
                //var codes = contentVar.Values.Take(contentVar.Values.Count).Select(value => value.Code).ToArray();
                //selection.ValueCodes.AddRange(codes);
                //selections.Add(selection);

                //// Time - Take the 12 last values
                //selection = new Selection(timeVar.Code);
                //var lstCodes = timeVar.Values.TakeLast(12).Select(value => value.Code).ToList();
                //lstCodes.Sort((a,b) => b.CompareTo(a)); // Descending sort
                //codes = lstCodes.ToArray();
                //selection.ValueCodes.AddRange(codes);
                //selections.Add(selection);
            }
            // 3 variables - Time, Content (with only 1 value) and one more
            else if (variablesWithSelection.Count == 3 && timeVar != null && contentVar != null && contentVar.Values.Count == 1)
            {

            }
            // 3 variables - Time, Content (with more than 1 value) and one more
            else if (variablesWithSelection.Count == 3 && timeVar != null && contentVar != null && contentVar.Values.Count > 1)
            {

            }
            // All other cases
            else
            {

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
