using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    [TestClass]
    public class MottakskvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : MottakskvitteringTests
        {
            [TestMethod]
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
                Assert.AreEqual(konversasjonsId, mottakskvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, mottakskvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, mottakskvittering.DigestValue);
            }
        }

        [TestClass]
        public class MottattMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var mottakskvittering = DomainUtility.GetMottakskvittering();

                //Act

                //Assert
                Assert.AreEqual(mottakskvittering.Generert, mottakskvittering.Mottatt);
            }
        }
    }
}