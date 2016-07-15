using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    public class VarslingFeiletKvitteringTests
    {
        public class KonstruktørMethod : VarslingFeiletKvitteringTests
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
                var varslingFeiletKvittering = new VarslingFeiletKvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.Equal(konversasjonsId, varslingFeiletKvittering.KonversasjonsId);
                Assert.Equal(bodyReferenceUri, varslingFeiletKvittering.BodyReferenceUri);
                Assert.Equal(digestValue, varslingFeiletKvittering.DigestValue);
            }
        }

        public class FeiletMethod : VarslingFeiletKvitteringTests
        {
            [Fact]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var varslingFeiletKvittering = DomainUtility.GetVarslingFeiletKvittering();

                //Act

                //Assert
                Assert.Equal(varslingFeiletKvittering.Generert, varslingFeiletKvittering.Feilet);
            }
        }
    }
}