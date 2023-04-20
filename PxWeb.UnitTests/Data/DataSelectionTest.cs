using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PCAxis.Paxiom;
using PxWeb.Api2.Server.Models;
using PxWeb.Code.Api2.Cache;
using PxWeb.Code.Api2.DataSelection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PxWeb.UnitTests.Data
{
    [TestClass]
    public class DataSelectionTest
    {
        [TestMethod]
        public void ShouldReturnSelectionForTimeAndContent()
        {
            PXModel model = GetPxModelWithTimeAndContent();
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Selection selection = selections.FirstOrDefault(s => s.VariableCode == "Period");
            Assert.AreEqual(12, selection.ValueCodes.Count);

            selection = selections.FirstOrDefault(s => s.VariableCode == "Content");
            Assert.AreEqual(2, selection.ValueCodes.Count);

            selection = selections.FirstOrDefault(s => s.VariableCode == "var1");
            Assert.AreEqual(0, selection.ValueCodes.Count);
        }


        private PXModel GetPxModelWithTimeAndContent()
        {
            PXModel pxModel = new PXModel();

            // Time
            Variable timeVar = new Variable("Period", PlacementType.Heading);
            timeVar.IsTime = true;
            timeVar.Elimination = false;

            timeVar.Values.Add(CreateValue("2017M10"));
            timeVar.Values.Add(CreateValue("2017M11"));
            timeVar.Values.Add(CreateValue("2017M12"));
            timeVar.Values.Add(CreateValue("2018M01"));
            timeVar.Values.Add(CreateValue("2018M02"));
            timeVar.Values.Add(CreateValue("2018M03"));
            timeVar.Values.Add(CreateValue("2018M04"));
            timeVar.Values.Add(CreateValue("2018M05"));
            timeVar.Values.Add(CreateValue("2018M06"));
            timeVar.Values.Add(CreateValue("2018M07"));
            timeVar.Values.Add(CreateValue("2018M08"));
            timeVar.Values.Add(CreateValue("2018M09"));
            timeVar.Values.Add(CreateValue("2018M10"));
            timeVar.Values.Add(CreateValue("2018M11"));
            timeVar.Values.Add(CreateValue("2018M12"));
            
            pxModel.Meta.AddVariable(timeVar);

            // Content
            Variable contentVar = new Variable("Content", PlacementType.Stub);
            contentVar.IsContentVariable = true;
            contentVar.Elimination = false;

            contentVar.Values.Add(CreateValue("CONTENT1"));
            contentVar.Values.Add(CreateValue("CONTENT2"));

            pxModel.Meta.AddVariable(contentVar);


            // Other variable that can be eliminated
            Variable elimVar = new Variable("var1", PlacementType.Stub);
            elimVar.Elimination = true;

            elimVar.Values.Add(CreateValue("Value1"));
            elimVar.Values.Add(CreateValue("Value2"));
            elimVar.Values.Add(CreateValue("Value3"));

            pxModel.Meta.AddVariable(elimVar);

            return pxModel;
        }

        private PCAxis.Paxiom.Value CreateValue(string code)
        {
            PCAxis.Paxiom.Value value = new PCAxis.Paxiom.Value(code);
            PaxiomUtil.SetCode(value, code);    
            return value;
        }


    }
}
