using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Transport
{
    public class TomKøKvitteringTests
    {
        public class KonstruktørMethod : TomKøKvitteringTests
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var tomKøKvittering = new TomKøKvittering();

                //Act

                //Assert
                Assert.Equal(string.Empty, tomKøKvittering.MeldingsId);
            }
        }
    }
}