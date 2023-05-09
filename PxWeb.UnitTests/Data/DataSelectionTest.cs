using DocumentFormat.OpenXml.Bibliography;
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
        public void GetSelectionShouldReturnEqualVariables()
        {
            PXModel model = GetPxModelDefaultSelection(2, 2);
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            variablesSelection = CreateVariablesSelection(variablesSelection);
            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Assert.AreEqual(variablesSelection.Selection.Select(x => x.VariableCode).Count(), selections.Count());
        }
        [TestMethod]
        public void GetSelectionShouldReturnZeroValues()
        {
            PXModel model = GetPxModelDefaultSelection(2, 2);
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            variablesSelection = CreateVariablesSelection(variablesSelection);
            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            var selection = selections.FirstOrDefault(s => s.VariableCode == "var3");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }
        }
        [TestMethod]
        public void ShouldReturnSelectionFor_Time_Content()
        {
            PXModel model = GetPxModelDefaultSelection(2, 2); // Only Time and content cannot be eliminated. Content has 2 values
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Selection? selection = selections.FirstOrDefault(s => s.VariableCode == "Period");
            if (selection != null)
            {
                Assert.AreEqual(12, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "Content");
            if (selection != null)
            {
                Assert.AreEqual(2, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var3");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var4");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var5");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }
        }

        [TestMethod]
        public void ShouldReturnSelectionFor_Time_ContentWithOneValue_OneMore()
        {
            PXModel model = GetPxModelDefaultSelection(3, 1); // Time, content and a third variable cannot be eliminated. Content has 1 value
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Selection? selection = selections.FirstOrDefault(s => s.VariableCode == "Period");
            if (selection != null)
            {
                Assert.AreEqual(12, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "Content");
            if (selection != null)
            {
                Assert.AreEqual(1, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var3");
            if (selection != null)
            {
                Assert.AreEqual(3, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var4");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var5");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }
        }

        [TestMethod]
        public void ShouldReturnSelectionFor_Time_ContentWithTwoValues_OneMore()
        {
            PXModel model = GetPxModelDefaultSelection(3, 2); // Time, content and a third variable cannot be eliminated. Content has 2 values
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Selection? selection = selections.FirstOrDefault(s => s.VariableCode == "Period");
            if (selection != null)
            {
                Assert.AreEqual(1, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "Content");
            if (selection != null)
            {
                Assert.AreEqual(2, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var3");
            if (selection != null)
            {
                Assert.AreEqual(3, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var4");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var5");
            if (selection != null)
            {
                Assert.AreEqual(0, selection.ValueCodes.Count);
            }

        }

        [TestMethod]
        public void ShouldReturnSelectionFor_AllVariables()
        {
            PXModel model = GetPxModelDefaultSelection(5, 2); // No variable can be eliminated. Content has 2 values
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            SelectionHandler selectionHandler = new SelectionHandler();

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);

            Selection? selection = selections.FirstOrDefault(s => s.VariableCode == "Period");
            if (selection != null)
            {
                Assert.AreEqual(1, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "Content");
            if (selection != null)
            {
                Assert.AreEqual(1, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var3");
            if (selection != null)
            {
                Assert.AreEqual(3, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var4");
            if (selection != null)
            {
                Assert.AreEqual(3, selection.ValueCodes.Count);
            }

            selection = selections.FirstOrDefault(s => s.VariableCode == "var5");
            if (selection != null)
            {
                Assert.AreEqual(1, selection.ValueCodes.Count);
            }

        }

        [TestMethod]
        public void ShouldReturnWildcardStarSelection()
        {
            List<string> valueCodes = new List<string>();

            valueCodes.Add("000*"); // 9 values
            valueCodes.Add("*100"); // 1 value

            var selections = GetSelection(valueCodes);

            var selection = selections.FirstOrDefault(s => s.VariableCode == "var1");
            if (selection != null)
            {
                Assert.AreEqual(10, selection.ValueCodes.Count);
            }
        }

        [TestMethod]
        public void ShouldReturnWildcardQuestionmarkSelection()
        {
            List<string> valueCodes = new List<string>();

            valueCodes.Add("00??"); // 98 values
            valueCodes.Add("0?00"); // 10 values

            var selections = GetSelection(valueCodes);

            var selection = selections.FirstOrDefault(s => s.VariableCode == "var1");
            if (selection != null)
            {
                Assert.AreEqual(108, selection.ValueCodes.Count);
            }
        }

        [TestMethod]
        public void ShouldReturnTopSelection()
        {
            List<string> valueCodes = new List<string>();

            valueCodes.Add("TOP(10)"); // 10 values

            var selections = GetSelection(valueCodes);

            var selection = selections.FirstOrDefault(s => s.VariableCode == "var1");
            if (selection != null)
            {
                Assert.AreEqual(10, selection.ValueCodes.Count);
            }
        }

        [TestMethod]
        public void ShouldReturnTopOffsetSelection()
        {
            List<string> valueCodes = new List<string>();

            valueCodes.Add("TOP(10,995)"); // 5 values

            var selections = GetSelection(valueCodes);

            var selection = selections.FirstOrDefault(s => s.VariableCode == "var1");
            if (selection != null)
            {
                Assert.AreEqual(5, selection.ValueCodes.Count);
            }
        }

        
        // Helper methods


        private PXModel GetPxModelDefaultSelection(int nonElimVariables, int contentValues)
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
            if (contentValues > 1)
            {
                contentVar.Values.Add(CreateValue("CONTENT2"));
            }

            pxModel.Meta.AddVariable(contentVar);


            // Variable 3
            Variable var3 = new Variable("var3", PlacementType.Stub);
            if (nonElimVariables == 2)
            {
                var3.Elimination = true;
            }
            else
            {
                var3.Elimination = false;
            }

            var3.Values.Add(CreateValue("Value1"));
            var3.Values.Add(CreateValue("Value2"));
            var3.Values.Add(CreateValue("Value3"));

            pxModel.Meta.AddVariable(var3);

            // Variable 4
            Variable var4 = new Variable("var4", PlacementType.Stub);
            if (nonElimVariables == 2 || nonElimVariables == 3)
            {
                var4.Elimination = true;
            }
            else
            {
                var4.Elimination = false;
            }

            var4.Values.Add(CreateValue("ValueA"));
            var4.Values.Add(CreateValue("ValueB"));
            var4.Values.Add(CreateValue("ValueC"));

            pxModel.Meta.AddVariable(var4);

            // Variable 5
            Variable var5 = new Variable("var5", PlacementType.Stub);
            if (nonElimVariables < 4)
            {
                var5.Elimination = true;
            }
            else
            {
                var5.Elimination = false;
            }

            var5.Values.Add(CreateValue("ValXXXX"));
            var5.Values.Add(CreateValue("ValXXXY"));
            var5.Values.Add(CreateValue("ValXXXZ"));
            var5.Values.Add(CreateValue("ValXXXZ"));

            pxModel.Meta.AddVariable(var5);
            return pxModel;
        }

        private PCAxis.Paxiom.Value CreateValue(string code)
        {
            PCAxis.Paxiom.Value value = new PCAxis.Paxiom.Value(code);
            PaxiomUtil.SetCode(value, code);
            return value;
        }

        private VariablesSelection CreateVariablesSelection(VariablesSelection variablesSelection)
        {
            //Add variable
            var variableSelectionObject = new VariableSelection
            {
                VariableCode = "var3",
                ValueCodes = new List<string>()
            };

            variablesSelection.Selection.Add(variableSelectionObject);

            return variablesSelection;

        }

        private VariableSelection CreateVariableSelection(string variableCode, List<string> valueCodes)
        {
            var variableSelectionObject = new VariableSelection
            {
                VariableCode = variableCode,
                ValueCodes = new List<string>()
            };

            variableSelectionObject.ValueCodes.AddRange(valueCodes);

            return variableSelectionObject;
        }


        private Selection[] GetSelection(List<string> wantedValues)
        {
            PXModel model = GetPxModelForTest();
            SelectionHandler selectionHandler = new SelectionHandler();
            VariablesSelection variablesSelection = new VariablesSelection();
            variablesSelection.Selection = new List<VariableSelection>();
            List<string> valueCodes = new List<string>();

            valueCodes.AddRange(wantedValues);

            var varSelection = CreateVariableSelection("var1", valueCodes);
            variablesSelection.Selection.Add(varSelection);

            Selection[] selections = selectionHandler.GetSelection(model, variablesSelection);
            return selections;
        }

        private PXModel GetPxModelForTest()
        {
            PXModel pxModel = new PXModel();

            Variable var1 = new Variable("var1", PlacementType.Heading);
            var1.Elimination = false;

            for (int i = 1; i <= 1000; i++)
            {
                var1.Values.Add(CreateValue(i.ToString("0000")));
            }

            pxModel.Meta.AddVariable(var1);

            return pxModel;

        }
    }
}
