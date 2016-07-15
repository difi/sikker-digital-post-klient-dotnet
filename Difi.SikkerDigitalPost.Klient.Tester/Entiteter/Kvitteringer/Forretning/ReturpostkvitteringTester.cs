using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class ReturpostkvitteringTests
    {
        public class KonstruktørMethod : ReturpostkvitteringTests
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
                var returpostkvittering = new Returpostkvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.Equal(konversasjonsId, returpostkvittering.KonversasjonsId);
                Assert.Equal(bodyReferenceUri, returpostkvittering.BodyReferenceUri);
                Assert.Equal(digestValue, returpostkvittering.DigestValue);
            }
        }

        public class ReturnertMethod : MottakskvitteringTests
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var returpostkvittering = DomainUtility.GetReturpostkvittering();

                //Act

                //Assert
                Assert.Equal(returpostkvittering.Generert, returpostkvittering.Returnert);
            }
        }
    }
}