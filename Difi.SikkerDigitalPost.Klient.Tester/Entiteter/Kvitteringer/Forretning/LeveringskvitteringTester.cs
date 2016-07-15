using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class LeveringskvitteringTests
    {
        public class KonstruktørMethod : LeveringskvitteringTests
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

        public class LevertMethod : MottakskvitteringTests
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var leveringskvittering = DomainUtility.GetLeveringskvittering();

                //Act

                //Assert
                Assert.Equal(leveringskvittering.Generert, leveringskvittering.Levert);
            }
        }
    }
}