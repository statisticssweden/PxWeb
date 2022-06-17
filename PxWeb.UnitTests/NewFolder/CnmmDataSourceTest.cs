using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Any;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PCAxis.Menu;
using Px.Abstractions.Interfaces;
using PxWeb.Code.Api2.DataSource;
using PxWeb.Code.Api2.DataSource.Cnmm;

namespace PxWeb.UnitTests.NewFolder
{
    [TestClass]
    public class CnmmDataSourceTest
    {
        [TestMethod]
        public void ShouldResolveItemCollection()
        {
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);
            
            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();

            var dict = new Dictionary<string, string>();

            dict.Add("1","Ett");
            dict.Add("2","Två");
            dict.Add("3","Tre");
            dict.Add("4","Fyra");
            dict.Add("5","Fem");
            dict.Add("6","Sex");

            pcAxisFactory.Setup(x => x.GetMenuLookup()).Returns(dict);
            
            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactory.Object);
            
            var result = resolver.Resolve("1");

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public void ShouldReturnMenu()
        {

           // var resolverMock = new Mock<IItemSelectionResolver>();

           // ItemSelection itemSelection = new ItemSelection();

           // resolverMock.Setup(x => x.Resolve(It.IsAny<string>())).Returns(itemSelection);
           ////resolverMock.Setup(x => x.Resolve(). )

           // var datasource = new CnmmDataSource(resolverMock.Object);
            
           // var result = datasource.CreateMenu("hej", "hej");




        }

    }
}
