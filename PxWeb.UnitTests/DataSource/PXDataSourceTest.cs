using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PCAxis.Sql.DbConfig;
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
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();

            var testFactory = new TestFactory();
            var dict = testFactory.GetMenuLookup();


            pcAxisFactory.Setup(x => x.GetMenuLookup(language)).Returns(dict);

            var resolver = new ItemSelectionResolverPxFile(memorymock.Object, pcAxisFactory.Object);

            bool selectionExists;

            var result = resolver.Resolve(language, "", out selectionExists);

            Assert.AreEqual("START", result.Menu);
            Assert.AreEqual("START", result.Selection);
        }

        [Ignore]
        [TestMethod]
        //[DeploymentItem(@"..\..\..\..\PxWeb\test.txt")]
        [DeploymentItem(@"..\..\..\..\PxWeb\wwwroot\Database\Menu.xml")]
        public void ShouldReturnMenu()
        {


            //var test= System.IO.File.ReadAllText("test.txt");

            //var test = System.IO.File.ReadAllText(@"..\..\..\..\PxWeb\test.txt");
            var test = System.IO.File.ReadAllText(@"..\..\..\..\PxWeb\wwwroot\Database\Menu.xml");

            //todo, mock database 
            string language = "en";
            var memorymock = new Mock<IMemoryCache>();
            var entryMock = new Mock<ICacheEntry>();
            memorymock.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(entryMock.Object);

            var configServiceMock = new Mock<IPxFileConfigurationService>();

            var pcAxisFactory = new Mock<IItemSelectionResolverFactory>();
            
            var logger = new Mock<ILogger>();
            var testFactory = new TestFactory();
//            var dict = testFactory.GetMenuLookup();

        var hostingEnvironmentMock = new Mock<IWebHostEnvironment>();

        


        var pcAxisFactoryNy =
            new ItemSelectorResolverPxFactory(configServiceMock.Object, hostingEnvironmentMock.Object, null);

            //hostingEnvironmentMock
            //    .Setup(m => m.EnvironmentName)
            //    .Returns("Hosting:UnitTestEnvironment");

            string fileName = "ich_will.mp3";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            var p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"JukeboxV2.0\JukeboxV2.0\Datos\ich will.mp3");


            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var file = Path.Combine(projectFolder, @"TestData\Test.xml");

            AppDomain root = AppDomain.CurrentDomain;

            string dataSource = AppDomain.CurrentDomain.BaseDirectory + "TestDb.mdb";

            var f = "test.txt";


            //DeploymentItemAttribute Class




            hostingEnvironmentMock
            .Setup(m => m.WebRootPath)
            .Returns("C:\\Users\\SCBMOSS\\source\\repos\\PxWeb_api2\\PxWeb\\wwwroot");

            

            //hostingEnvironment.Setup(
            //x => x.WebRootPath() = $"C:\\Users\\SCBMOSS\\source\\repos\\PxWeb_api2\\PxWeb\\wwwroot\\Database");
            //    // pcAxisFactory.Setup(x => x.GetMenuLookup(language)).Returns(dict);

            var resolver = new ItemSelectionResolverCnmm(memorymock.Object, pcAxisFactoryNy);

            var datasource = new PxFileDataSource(configServiceMock.Object, resolver, hostingEnvironmentMock.Object);

            bool selectionExists;

            var result = datasource.CreateMenu("alias", language, out selectionExists);

            Assert.IsNotNull(result);
        }




    }
}
