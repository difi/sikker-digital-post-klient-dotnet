using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class MottakskvitteringTests
    {
        public class KonstruktørMethod : MottakskvitteringTests
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
                var mottakskvittering = new Mottakskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.Equal(konversasjonsId, mottakskvittering.KonversasjonsId);
                Assert.Equal(bodyReferenceUri, mottakskvittering.BodyReferenceUri);
                Assert.Equal(digestValue, mottakskvittering.DigestValue);
            }
        }

        public class MottattMethod : MottakskvitteringTests
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var mottakskvittering = DomainUtility.GetMottakskvittering();

                //Act

                //Assert
                Assert.Equal(mottakskvittering.Generert, mottakskvittering.Mottatt);
            }
        }
    }
}