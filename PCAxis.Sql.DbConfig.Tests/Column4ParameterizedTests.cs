using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PCAxis.Sql.DbConfig;
using PCAxis.Sql.Exceptions;
using System.Collections.Specialized;
using System.Data.Common;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace PCAxis.Sql.DbConfig.Tests
{
    [TestClass]
    public class Column4ParameterizedTests
    {

        private Column4Parameterized col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Oracle");

        [TestMethod]
        [TestCategory("Unit")]
        public void PureColumnName_GivenColName_ReturnsThisColName()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", null, null, null);

            //Act
            var result = col4Para.PureColumnName();
       
            //Assert
            var expectedResult = "ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Id_GivenColNameAndTableAlias_ReturnsThisId()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);

            //Act
            var result = col4Para.Id();

            //Assert
            var expectedResult = "TableAlias.ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Label_GivenColNameAndTableAlias_ReturnsThisLabel()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);

            //Act
            var result = col4Para.Label();

            //Assert
            var expectedResult = "TableAlias_ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Label_GivenColName_ReturnsThisLabel2()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", null, null, null);

            //Act
            var result = col4Para.Label();

            //Assert
            var expectedResult = "_ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ForSelect_GivenColNameAndTableAlias_ReturnsSQLPartExpressionForNamingWithLabel()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);

            //Act
            var result = col4Para.ForSelect();

            //Assert
            var expectedResult = "TableAlias.ColName AS TableAlias_ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTableColAndParaMeterRef_ReturnsThisIDEqualsInputSQLPartExpression()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);
            var parameterRef = "someParameterRef";

            //Act
            var result = col4Para.Is(parameterRef);

            //Assert
            var expectedResult = "TableAlias.ColName = someParameterRef";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenCompleteCol4Para_ReturnsThisIDEqualsParameterReferenceSQLPartExpression_Oracle()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Oracle");

            //Act
            var result = col4Para.Is();

            //Assert
            var expectedResult = "TableAlias.ColName = :aModelName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenCompleteCol4Para_ReturnsThisIDEqualsParameterReferenceSQLPartExpression_SQL()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Sql");

            //Act
            var result = col4Para.Is();

            //Assert
            var expectedResult = "TableAlias.ColName = @aModelName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenCompleteCol4Para_ReturnsThisIDEqualsParameterReferenceSQLPartExpression_OLEDB()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "OLEDB");

            //Act
            var result = col4Para.Is();

            //Assert
            var expectedResult = "TableAlias.ColName = ?";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenCompleteCol4Para_ReturnsThisIDEqualsParameterReferenceSQLPartExpression_ODBC()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "ODBC");

            //Act
            var result = col4Para.Is();

            //Assert
            var expectedResult = "TableAlias.ColName = ?";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenIncorrectDB_ReturnsException()
        {
            //Assert
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "notDBString");

            //Act & Assert
            try 
            { 
                var result = col4Para.Is();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exceptions.ConfigException);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsUppered_GivenCorrectDBAndFullCol4Para_ReturnsSQLPartExpressionToUpper_Oracle()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Oracle");

            //Act
            var result = col4Para.IsUppered();

            //Assert
            var expectedResult = "upper(TableAlias.ColName) = upper(:aModelName)";
            Assert.AreEqual(expectedResult, result);

        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsUppered_GivenInCorrectDBAndFullCol4Para_ReturnsException()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "notAnDB");

            //Act & Assert
            try
            {
                var result = col4Para.IsUppered();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exceptions.ConfigException);
            }

        }


        [TestMethod]
        [TestCategory("Unit")]
        public void Like_GivenCorrectDBType_ReturnsLikeSQLPartExpression_Oracle()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Oracle");

            //Act
            var result = col4Para.Like();

            //Assert
            var expectedResult = "TableAlias.ColName LIKE :aModelName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Like_GivenCorrectDBType_ReturnsLikeSQLPartExpression_Sql()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "Sql");

            //Act
            var result = col4Para.Like();

            //Assert
            var expectedResult = "TableAlias.ColName LIKE @aModelName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Like_GivenInCorrectDBType_ReturnsException()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", "ModelName", "noDBName");

            //Act & Assert
            try
            {
                var result = col4Para.Like();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exceptions.ConfigException);
            }
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void In_GivenCorrectDBTypeAndQuestionMarkAsParaMeterRefBase_ReturnsInSQLPartExpression()
        {
            var parameterRefBase = "?";
            var numberOfValues = 5;

            var result = col4Para.In(parameterRefBase, numberOfValues);

            var expectedResult = "TableAlias.ColName IN (?, ?, ?, ?, ?)";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTwoCol4Para_ReturnsEqualsSQLPartExpression()
        { 
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);
            var col4Para2 = new Column4Parameterized("ColName", "TableAlias2", null, null);

            //Act
            var result = col4Para.Is(col4Para2);

            //Assert
            var expectedResult = "TableAlias.ColName = TableAlias2.ColName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void Is_GivenTableColAndLang_ReturnsIdEqualLandIdSQLPartExpresssion()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);
            var lang2Col4Para = new Lang2Column4Parameterized("ColName", "TableAlias", new Dictionary<string, string> { { "no", "norsk" }, { "dk", "dansk" } });
            var lang = "no";

            //Act
            var result = col4Para.Is(lang2Col4Para, lang);

            //Assert
            var expectedResult = "TableAlias.ColName = TableAliasnorsk.ColName";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void IsNotNULL_GivenTableCol_ReturnsIdIsNotNullSQLPartExpresssion()
        {
            //Arrange
            var col4Para = new Column4Parameterized("ColName", "TableAlias", null, null);

            //Act
            var result = col4Para.IsNotNULL();

            //Assert
            var expectedResult = "TableAlias.ColName IS NOT NULL";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void In_GivenCorrectDBType_ReturnsInSQLPartException()
        {
            var listOfValues = new StringCollection();
            listOfValues.AddRange(new String[] { "value1", "value2", "value3" });

            var result = col4Para.In(listOfValues);

            var expectedResult = "( (TableAlias.ColName IN ( :aModelName1,:aModelName2,:aModelName3)))";
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void GetParameterReference_GivenValidDBType_ReturnsCorrectSQLPartExpression()
        {
            //Arrange
            var dataProvider = "oRacLE";
            var propertyName = "myPropertyName";
            var args = new object[2] { propertyName, dataProvider };
            PrivateType col4ParaType = new PrivateType(col4Para.GetType());

            //Act
            var result = col4ParaType.InvokeStatic("GetParameterReference", args);
            
            //Assert
            var expectedResult = ":amyPropertyName";
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetParameterReference_GivenIncorrectDBType_ReturnsException()
        {
            var dataProvider = "oraCe";
            var propertyName = "myPropertyName";
            var args = new object[2] { propertyName, dataProvider };
            PrivateType col4ParaType = new PrivateType(col4Para.GetType());

            try 
            { 
                col4ParaType.InvokeStatic("GetParameterReference", args);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exceptions.ConfigException);
            }
            
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetEmptyDbParameter_GivenIncorrectDBType_ReturnsException()
        {

            var dataProvider = "oraCe";
            var args = new object[1] { dataProvider };
            PrivateType col4ParaType = new PrivateType(col4Para.GetType());

            try
            {
                col4ParaType.InvokeStatic("GetEmptyDbParameter", args);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is Exceptions.ConfigException);
            }
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void In_GivenCorrectDBTypeAndNotQuestionMarkAsParaMeterRefBase_ReturnsInSQLPartException()
        {
            var parameterRefBase = ":";
            var numberOfValues = 5;

            var result = col4Para.In(parameterRefBase, numberOfValues);

            var expectedResult = "TableAlias.ColName IN (:1, :2, :3, :4, :5)";
            Assert.AreEqual(expectedResult, result);
        }

    }
}
