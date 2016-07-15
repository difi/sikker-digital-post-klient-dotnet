using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    public class TransportOkKvitteringTests
    {
        public class KonstruktørMethod : TransportOkKvitteringTests
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var transportOkKvittering = new TransportOkKvittering();

                //Act

                //Assert
                Assert.Equal(string.Empty, transportOkKvittering.MeldingsId);
            }
        }
    }
}