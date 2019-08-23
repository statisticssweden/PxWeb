using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCAxis.Sql.DbConfig;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace PCAxis.Sql.DbConfig.Tests
{
    [TestClass]
    public class ColTests
    {

        [TestMethod]
        [TestCategory("Unit")]
        public void Id_GivenTableCol_ReturnsIdCombinedNameSeperatedbyDot()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");

            //Act
            var result = col.Id();

            //Assert
            var expectedResult = "TableAlias.ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Label_GivenTableCol_ReturnsLabelCombinedNameSeperatedByUnderscore()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");

            //Act
            var result = col.Label();

            //Assert
            var expectedResult = "TableAlias_ColName";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void ForSelect_GivenTableCol_ReturnsSQLPartExpressionForLabeling()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");

            //Act
            var result = col.ForSelect();

            //Assert
            var expectedResult = "TableAlias.ColName AS TableAlias_ColName";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTableColAndFilter_SoundSQLFilterPartExpression()
        {
            //Arrange
            var col = new Col("SuperCol", "SuperTable");
            var filterValue = "NiceValue";

            //Act
            var isExpression = col.Is(filterValue);

            //Assert
            var expectedIsExpression = "SuperTable.SuperCol = 'NiceValue'";
            Assert.AreEqual(expectedIsExpression, isExpression);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Like_GivenTableCol_ReturnsSQLPartExpressionForContainsComparisson()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");
            var wildCard = "myWildCard";

            //Act
            var result = col.Like(wildCard);

            //Assert
            var expectedResult = "TableAlias.ColName LIKE 'myWildCard'";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void In_GivenTableCol_ReturnsInSQLPartExpression()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");
            var listOfValues = new String[] { "value1", "value2", "value3" };

            //Act
            var expression = col.In(listOfValues);

            //Assert
            var expectedExpression = "(TableAlias.ColName IN ('value1', 'value2', 'value3'))";
            Assert.AreEqual(expectedExpression, expression);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsUppered_GivenTableCol_ReturnsUpperedSQLPartExpresssion()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");
            var constantValue = "someConstantValue";

            //Act
            var expression = col.IsUppered(constantValue);

            //Assert
            var expectedExpression = "upper(TableAlias.ColName) = upper('someConstantValue')";
            Assert.AreEqual(expectedExpression, expression);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTwoCol_ReturnsIdEqualSQLPartExpresssion()
        {
            //Arrange 
            var col = new Col("ColName", "TableAlias");
            var col2 = new Col("ColName", "TableAlias2");

            //Act
            var expression = col.Is(col2);

            //Assert
            var expectedExpression = "TableAlias.ColName = TableAlias2.ColName";
            Assert.AreEqual(expectedExpression, expression);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTableColAndLang_ReturnsIdEqualLandIdSQLPartExpresssion()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");
            var lang2Col = new Lang2Col("ColName", "TableAlias", new Dictionary<string, string> { { "no", "norsk" }, { "dk", "dansk" } });
            var lang = "no";

            //Act
            var result = col.Is(lang2Col, lang);

            //Assert
            var expectedResult = "TableAlias.ColName = TableAliasnorsk.ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsNotNULL_GivenTableCol_ReturnsIdIsNotNullSQLPartExpresssion()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");

            //Act
            var result = col.IsNotNULL();

            //Assert
            var expectedResult = "TableAlias.ColName IS NOT NULL";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsNotNULL_GivenTableCol_ReturnsIdIsNotNullSQLPartExpresssionUpperCase()
        {
            //Arrange
            var col = new Col("ColName", "TableAlias");

            //Act
            var result = col.IsNotNULL();

            //Assert
            var notUpperCaseResult = "TableAlias.ColName IS not NULL";
            Assert.AreNotEqual(notUpperCaseResult, result);
        }


      
    }
}
