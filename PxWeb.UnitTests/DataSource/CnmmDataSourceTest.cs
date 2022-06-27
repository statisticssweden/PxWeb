using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PxWeb.Code.Api2.DataSource.Cnmm;

namespace PxWeb.UnitTests.DataSource
{
    [TestClass]
    public class CnmmDataSourceTest
    {
        [TestMethod]
        public void ResolveShouldResolveItemCollection()
        {
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);
            
            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();

            var testFactory = new TestFactory();
            var dict = testFactory.GetMenuLookup();
            
            pcAxisFactory.Setup(x => x.GetMenuLookup(language)).Returns(dict);
            
            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactory.Object);
            
            var result = resolver.Resolve(language, "AA0003");

            Assert.IsNotNull(result);
            Assert.AreEqual("AA", result.Menu);
        }


        [TestMethod]
        public void ResolveEmtySelectionItemShouldReturnStart()
        {
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();

            var testFactory = new TestFactory();
            var dict = testFactory.GetMenuLookup();


            pcAxisFactory.Setup(x => x.GetMenuLookup(language)).Returns(dict);

            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactory.Object);

            var result = resolver.Resolve(language, "");

            Assert.AreEqual("START",result.Menu );
            Assert.AreEqual("START", result.Selection);
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
