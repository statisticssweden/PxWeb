using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PxWeb.Code.Api2.DataSource.Cnmm;
using PxWeb.Code.Api2.DataSource.PxFile;
using PxWeb.Config.Api2;

namespace PxWeb.UnitTests.DataSource
{
    [TestClass]
    public class PXDataSourceTest
    {

        [TestMethod]
        public void ResolveEmtySelectionItemShouldReturnStart()
        {
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            var configMock = new Mock<IPxApiConfigurationService>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();

            var testFactory = new TestFactory();
            var dict = testFactory.GetMenuLookup();

            var config = testFactory.GetPxApiConfiguration();
            configMock.Setup(x => x.GetConfiguration()).Returns(config);

            pcAxisFactory.Setup(x => x.GetMenuLookup(language)).Returns(dict);

            var resolver = new ItemSelectionResolverPxFile(memorymock.Object, pcAxisFactory.Object, configMock.Object);

            bool selectionExists;

            var result = resolver.Resolve(language, "", out selectionExists);

            Assert.AreEqual("START", result.Menu);
            Assert.AreEqual("START", result.Selection);
        }

       
        [TestMethod]
        [DeploymentItem(@"Database")]
        public void ShouldReturnMenu()
        {
            //todo, mock database 
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            var configMock = new Mock<IPxApiConfigurationService>();

            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            
            var pcAxisFactory = new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);
            
            hostingEnvironmentMock
            .Setup(m => m.WebRootPath)
            .Returns("");

            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactory);
            var datasource = new PxFileDataSource(configServiceMock.Object, resolver, hostingEnvironmentMock.Object);
            bool selectionExists;
            
            var result = datasource.CreateMenu("", language, out selectionExists);
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [DeploymentItem(@"Database")]
        public void ResolveShouldResolveItemCollection()
        {
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            var pcAxisFactory = new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);

            hostingEnvironmentMock
                .Setup(m => m.WebRootPath)
                .Returns("");

            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactory);
            
            bool selectionExists;

            var result = resolver.Resolve(language, "ALIAS", out selectionExists);

            Assert.IsNotNull(result);
            Assert.AreEqual("Example", result.Menu);
            Assert.IsTrue(selectionExists);
        }



    }
}
