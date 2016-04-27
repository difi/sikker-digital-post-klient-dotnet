using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Resources.Xml;
using Difi.SikkerDigitalPost.Klient.Tester.Fakes;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal
{
    [TestClass]
    public class RequestHelperTests
    {
        [TestClass]
        public class ConstructorMethod : RequestHelperTests
        {
            [TestMethod]
            public void InitializesFields()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                var requestHelper = new RequestHelper(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, requestHelper.ClientConfiguration);
            }
        }

        [TestClass]
        public class SendMethod : RequestHelperTests
        {
            [TestMethod]
            public async Task ReturnsReceiptSuccessfully()
            {
                //Arrange
                var forretningsmeldingEnvelope = DomainUtility.GetForretningsmeldingEnvelope();

                var documentBundle = AsiceGenerator.Create(DomainUtility.GetForsendelseSimple(), new GuidUtility(), DomainUtility.GetAvsenderCertificate(), DomainUtility.GetKlientkonfigurasjon());

                var requestHelper = new RequestHelper(new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø));
                var fakeHttpClientHandlerResponse = new FakeHttpClientHandlerResponse(XmlResource.Response.GetTransportOk().OuterXml, HttpStatusCode.OK);
                requestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act 
                var kvittering = await requestHelper.SendMessage(forretningsmeldingEnvelope, documentBundle);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof (TransportOkKvittering));
            }
        }
    }
}