using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Resources.Xml;
using Difi.SikkerDigitalPost.Klient.Tester.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Fakes;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Api
{
    [TestClass]
    public class SikkerDigitalPostKlientTests
    {
        [TestClass]
        public class ConstructorMethod : SikkerDigitalPostKlientTests
        {
            [TestMethod]
            public void InitializesFields()
            {
                //Arrange
                var databehandler = new Databehandler(new Organisasjonsnummer("999999999"), DomainUtility.GetAvsenderCertificate());
                var klientkonfigurasjon = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(databehandler, klientkonfigurasjon);

                //Assert
                Assert.AreEqual(klientkonfigurasjon, sikkerDigitalPostKlient.Klientkonfigurasjon);
                Assert.AreEqual(databehandler, sikkerDigitalPostKlient.Databehandler);
                Assert.IsInstanceOfType(sikkerDigitalPostKlient.RequestHelper, typeof (RequestHelper));
            }
        }

        [TestClass]
        public class SendMethod : SikkerDigitalPostKlientTests
        {
            [TestMethod]
            public async Task SuccessfullyReturnsTransportErrorReceipt()
            {
                //Arrange
                var sikkerDigitalPostKlient = DomainUtility.GetSikkerDigitalPostKlientQaOffentlig();
                var fakeHttpClientHandlerResponse = new FakeHttpClientHandlerResponse(XmlResource.Response.GetTransportError().OuterXml, HttpStatusCode.BadRequest);
                sikkerDigitalPostKlient.RequestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act
                var forsendelse = DomainUtility.GetForsendelseSimple();
                var transportkvittering = await sikkerDigitalPostKlient.SendAsync(forsendelse);

                //Assert
                Assert.IsInstanceOfType(transportkvittering, typeof (TransportFeiletKvittering));
            }

            [TestMethod]
            public async Task SuccessfullyCallsAllDokumentpakkeProsessors()
            {
                //Arrange
                var klientkonfigurasjon = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø)
                {
                    Dokumentpakkeprosessorer = new List<IDokumentpakkeProsessor>
                    {
                        new SimpleDocumentBundleProcessor(),
                        new SimpleDocumentBundleProcessor()
                    }
                };

                var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(DomainUtility.GetDatabehandler(), klientkonfigurasjon);

                DomainUtility.GetSikkerDigitalPostKlientQaOffentlig();
                var fakeHttpClientHandlerResponse = new FakeHttpClientHandlerResponse(XmlResource.Response.GetTransportOk().OuterXml, HttpStatusCode.OK);
                sikkerDigitalPostKlient.RequestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act
                var forsendelse = DomainUtility.GetForsendelseSimple();

                try
                {
                    await sikkerDigitalPostKlient.SendAsync(forsendelse);
                }
                catch (SdpSecurityException)
                {
                    /*
                        We do not care about the results. Just do sending. Nasty hack as we are unable to mock validation 
                        in SikkerDigitalPostKlient, which results in invalid timestamp since response is out of date.
                    */
                }

                //Assert
                foreach (var dokumentpakkeProsessor in klientkonfigurasjon.Dokumentpakkeprosessorer.Cast<SimpleDocumentBundleProcessor>())
                {
                    Assert.IsTrue(dokumentpakkeProsessor.CouldReadBytesStream);
                }
            }

            [ExpectedException(typeof (SdpSecurityException))]
            [TestMethod]
            public async Task ThrowsExceptionOnResponseNotMatchingRequest()
            {
                //Arrange
                var sikkerDigitalPostKlient = DomainUtility.GetSikkerDigitalPostKlientQaOffentlig();
                var fakeHttpClientHandlerResponse = new FakeHttpClientHandlerResponse(XmlResource.Response.GetTransportOk().OuterXml, HttpStatusCode.OK);
                sikkerDigitalPostKlient.RequestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act
                var forsendelse = DomainUtility.GetForsendelseSimple();
                var transportkvittering = await sikkerDigitalPostKlient.SendAsync(forsendelse);

                //Assert
                Assert.IsInstanceOfType(transportkvittering, typeof (TransportFeiletKvittering));
            }
        }
    }
}