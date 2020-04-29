using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Tester.Fakes;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using SecurityException = Difi.SikkerDigitalPost.Klient.Domene.Exceptions.SecurityException;

namespace Difi.SikkerDigitalPost.Klient.Tester.Api
{
    public class SikkerDigitalPostKlientTests
    {
        public class ConstructorMethod : SikkerDigitalPostKlientTests
        {
            [Fact]
            public void Initializes_fields()
            {
                //Arrange
                var databehandler = new Databehandler(DomainUtility.Organisasjonsnummer());
                var klientkonfigurasjon = new Klientkonfigurasjon(Miljø.IntegrasjonsPunktLocalHostMiljø);

                //Act
                var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
                var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(databehandler, klientkonfigurasjon, serviceProvider.GetService<ILoggerFactory>());

                //Assert
                Assert.Equal(klientkonfigurasjon, sikkerDigitalPostKlient.Klientkonfigurasjon);
                Assert.Equal(databehandler, sikkerDigitalPostKlient.Databehandler);
                Assert.IsType<RequestHelper>(sikkerDigitalPostKlient.RequestHelper);
            }
        }

        public class SendMethod : SikkerDigitalPostKlientTests
        {
            [Fact]
            public async Task Returns_transport_error_receipt()
            {
                //Arrange
                var sikkerDigitalPostKlient = DomainUtility.GetSikkerDigitalPostKlientIPLocalHost();
                var fakeHttpClientHandlerResponse = new FakeResponseHandler()
                {
                    //TODO: Fix test
                    HttpContent = new StringContent(""),
                    StatusCode =  HttpStatusCode.BadRequest
                };
                sikkerDigitalPostKlient.RequestHelper.HttpClient = new HttpClient(fakeHttpClientHandlerResponse);

                //Act
                var forsendelse = DomainUtility.GetForsendelseSimple();
                var transportkvittering = await sikkerDigitalPostKlient.SendAsync(forsendelse).ConfigureAwait(false);

                //Assert
                Assert.IsType<TransportFeiletKvittering>(transportkvittering);
            }
        }
    }
}
