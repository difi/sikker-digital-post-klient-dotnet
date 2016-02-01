using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class VarslingFeiletKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : ReturpostkvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var konversasjonsId = Guid.NewGuid();
                var bodyReferenceUri = "bodyReferenceUri";
                var digestValue = "digestValue";

                //Act
                var varslingFeiletKvittering = new VarslingFeiletKvittering(konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, varslingFeiletKvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, varslingFeiletKvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, varslingFeiletKvittering.DigestValue);
            }
        }

        [TestClass]
        public class FeiletMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var mottakskvittering = DomeneUtility.GetVarslingFeiletKvittering();

                //Act

                //Assert
                Assert.AreEqual(mottakskvittering.Generert, mottakskvittering.Feilet);
            }
        }
    }
}