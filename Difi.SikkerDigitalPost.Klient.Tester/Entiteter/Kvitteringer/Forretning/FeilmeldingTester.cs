using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class FeilmeldingTester
    {
        public class KonstruktørMethod : FeilmeldingTester
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var meldingsId = "meldingsId";
                var konversasjonsId = Guid.NewGuid();
                var bodyReferenceUri = "bodyReferenceUri";
                var digestValue = "digestValue";

                //Act
                var feilmelding = new Feilmelding(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.Equal(konversasjonsId, feilmelding.KonversasjonsId);
                Assert.Equal(bodyReferenceUri, feilmelding.BodyReferenceUri);
                Assert.Equal(digestValue, feilmelding.DigestValue);
            }
        }

        public class FeiletMethod : MottakskvitteringTests
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var feilmelding = DomainUtility.GetFeilmelding();

                //Act

                //Assert
                Assert.Equal(feilmelding.Generert, feilmelding.Feilet);
            }
        }
    }
}