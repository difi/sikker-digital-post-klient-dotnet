using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class ÅpningskvitteringTester
    {
        public class KonstruktørMethod : ÅpningskvitteringTester
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var meldingsId = "MeldingsId";
                var konversasjonsId = Guid.NewGuid();
                var bodyReferenceUri = "bodyReferenceUri";
                var digestValue = "digestValue";

                //Act
                var leveringskvittering = new Leveringskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.Equal(konversasjonsId, leveringskvittering.KonversasjonsId);
                Assert.Equal(bodyReferenceUri, leveringskvittering.BodyReferenceUri);
                Assert.Equal(digestValue, leveringskvittering.DigestValue);
            }
        }

        public class ÅpnetMethod : ÅpningskvitteringTester
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var åpningskvittering = DomainUtility.GetÅpningskvittering();

                //Act

                //Assert
                Assert.Equal(åpningskvittering.Generert, åpningskvittering.Åpnet);
            }
        }
    }
}