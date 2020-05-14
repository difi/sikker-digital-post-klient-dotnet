using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Tester.Api;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal
{
    public class RequestHelperTests
    {
        public class ConstructorMethod : RequestHelperTests
        {
            [Fact]
            public void Initializes_fields()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(SikkerDigitalPostKlientTests.IntegrasjonsPunktLocalHostMiljø);

                //Act
                var requestHelper = new RequestHelper(clientConfiguration, new NullLoggerFactory());

                //Assert
                Assert.Equal(clientConfiguration, requestHelper.ClientConfiguration);
            }
        }
    }
}
