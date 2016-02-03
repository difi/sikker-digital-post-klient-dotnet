using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    [TestClass]
    public class ÅpningskvitteringTester
    {
        [TestClass]
        public class KonstruktørMethod : ÅpningskvitteringTester
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
                var leveringskvittering = new Leveringskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, leveringskvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, leveringskvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, leveringskvittering.DigestValue);
            }
        }

        [TestClass]
        public class ÅpnetMethod : ÅpningskvitteringTester
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var åpningskvittering = DomeneUtility.GetÅpningskvittering();

                //Act

                //Assert
                Assert.AreEqual(åpningskvittering.Generert, åpningskvittering.Åpnet);
            }
        }

    }
}
