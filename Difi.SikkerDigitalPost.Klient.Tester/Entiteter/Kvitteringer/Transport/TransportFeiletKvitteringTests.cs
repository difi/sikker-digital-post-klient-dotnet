using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    public class TransportFeiletKvitteringTests
    {
        public class KonstruktørMethod : TransportFeiletKvitteringTests
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var transportFeiletKvittering = new TransportFeiletKvittering();

                //Act

                //Assert
                Assert.Equal(string.Empty, transportFeiletKvittering.MeldingsId);
            }
        }
    }
}