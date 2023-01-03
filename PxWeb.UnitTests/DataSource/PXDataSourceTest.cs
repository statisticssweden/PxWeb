using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

        internal static string GetFullPathToFile(string pathRelativeUnitTestingFile)
        {
            string folderProjectLevel = GetPathToPxWebProject();
            string final = System.IO.Path.Combine(folderProjectLevel, pathRelativeUnitTestingFile);
            return final;
        }

        private static string GetPathToPxWebProject()
        {
            string pathAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string folderAssembly = System.IO.Path.GetDirectoryName(pathAssembly);
            if (folderAssembly.EndsWith("\\") == false) folderAssembly = folderAssembly + "\\";
            string folderProjectLevel = System.IO.Path.GetFullPath(folderAssembly + "..\\..\\..\\..\\");
            return folderProjectLevel;
        }

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
        public void ShouldReturnMenu()
        {
            
            //arrange
            var testFactory = new TestFactory();
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            var configMock = new Mock<IPxApiConfigurationService>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            var loggerMock = new Mock<ILogger<TablePathResolverPxFile>>();
            
            var config = testFactory.GetPxApiConfiguration();
            configMock.Setup(x => x.GetConfiguration()).Returns(config);

            var pcAxisFactory = new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);

            var wwwrootPath = GetFullPathToFile(@"PxWeb\wwwroot\");

            hostingEnvironmentMock
                .Setup(m => m.WebRootPath)
                .Returns(wwwrootPath);

            var resolver = new ItemSelectionResolverCnmm( memorymock.Object, pcAxisFactory, configMock.Object);
            var tablePathResolver = new TablePathResolverPxFile(memorymock.Object, hostingEnvironmentMock.Object, configMock.Object, loggerMock.Object);
            var datasource = new PxFileDataSource(configServiceMock.Object, resolver, tablePathResolver, hostingEnvironmentMock.Object);
            bool selectionExists;

            //act
            var result = datasource.CreateMenu("", language, out selectionExists);

            //assert
            Assert.IsNotNull(result);
        }
        
        [TestMethod]
        public void ResolveShouldResolveItemCollection()
        {
            var testFactory = new TestFactory();
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            var configMock = new Mock<IPxApiConfigurationService>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

            var config = testFactory.GetPxApiConfiguration();
            configMock.Setup(x => x.GetConfiguration()).Returns(config);

            var pcAxisFactory = new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);

            var wwwrootPath = GetFullPathToFile(@"PxWeb\wwwroot\");

            hostingEnvironmentMock
                .Setup(m => m.WebRootPath)
                .Returns(wwwrootPath);

            var resolver = new ItemSelectionResolverPxFile(memorymock.Object, pcAxisFactory, configMock.Object);

            bool selectionExists;

            var result = resolver.Resolve(language, "ALIAS", out selectionExists);

            Assert.IsNotNull(result);
            Assert.AreEqual("Database", result.Menu);
            Assert.IsTrue(selectionExists);
        }

        [TestMethod]
        public void ShouldResolveTablePath()
        {
            string language = "en";
            var resolver = GetTablePathResolver();

            bool selectionExists;

            var result = resolver.Resolve(language, "officialstatistics.px", out selectionExists);

            Assert.IsNotNull(result);
            Assert.IsTrue(selectionExists);
        }

        [TestMethod]
        public void ShouldNotResolveTablePath()
        {
            string language = "en";
            var resolver = GetTablePathResolver();

            bool selectionExists;

            var result = resolver.Resolve(language, "officialstatistics2.px", out selectionExists);

            Assert.AreEqual("", result);
            Assert.IsFalse(selectionExists);
        }

        private TablePathResolverPxFile GetTablePathResolver()
        {
            var testFactory = new TestFactory();
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            var configMock = new Mock<IPxApiConfigurationService>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();
            var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            var loggerMock = new Mock<ILogger<TablePathResolverPxFile>>();


            var config = testFactory.GetPxApiConfiguration();
            configMock.Setup(x => x.GetConfiguration()).Returns(config);

            var pcAxisFactory = new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);

            var wwwrootPath = GetFullPathToFile(@"PxWeb\wwwroot\");

            hostingEnvironmentMock
                .Setup(m => m.WebRootPath)
                .Returns(wwwrootPath);

            var resolver = new TablePathResolverPxFile(memorymock.Object, hostingEnvironmentMock.Object, configMock.Object, loggerMock.Object);

            return resolver;
        }
    }
}
