using System;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class VarslingFeiletKvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : VarslingFeiletKvitteringTests
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
        public class FeiletMethod : VarslingFeiletKvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var varslingFeiletKvittering = DomeneUtility.GetVarslingFeiletKvittering();

                //Act

                //Assert
                Assert.AreEqual(varslingFeiletKvittering.Generert, varslingFeiletKvittering.Feilet);
            }
        }
    }
}