using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using PCAxis.Paxiom;
using PxWeb.Code.Api2.Serialization;

namespace PxWeb.UnitTests.Serialization
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void ShouldSerializeToPx()
        {
            PXModel pxModel = TestFactory.GetPxModel();
            
            var serializeManager = new SerializeManager();
            string outputFormat = "px";
            var serializer = serializeManager.GetSerializer(outputFormat);

            Assert.AreEqual(serializer.GetType().Name, "PxDataSerializer");
        }

        [TestMethod]
        public void ShouldSerializeToPxFomrat()
        {
            PXModel pxModel = TestFactory.GetPxModel();

            var serializeManager = new SerializeManager();
            string outputFormat = "px";
            var serializer = serializeManager.GetSerializer(outputFormat);


            //var expected = "response content";
            //var expectedBytes = Encoding.UTF8.GetBytes(expected);
            //var responseStream = new MemoryStream();
            //responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            //responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<HttpResponse>();
            //response.Setup(c => c.GetResponseStream()).Returns(responseStream);
            response.Setup(x => x.StatusCode).Returns(1);
            
            //var serializer = _serializeManager.GetSerializer(outputFormat);
            serializer.Serialize(pxModel, response.Object);

            Assert.AreEqual(serializer.GetType().Name, "PxDataSerializer");
        }


    }
}
