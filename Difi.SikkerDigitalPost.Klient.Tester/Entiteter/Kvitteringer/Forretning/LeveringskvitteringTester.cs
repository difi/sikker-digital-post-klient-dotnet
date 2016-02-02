using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    [TestClass]
    public class LeveringskvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : LeveringskvitteringTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var konversasjonsId = Guid.NewGuid();
                var bodyReferenceUri = "bodyReferenceUri";
                var digestValue = "digestValue";

                //Act
                var leveringskvittering = new Leveringskvittering(konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, leveringskvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, leveringskvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, leveringskvittering.DigestValue);
            }
        }

        [TestClass]
        public class LevertMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var leveringskvittering = DomeneUtility.GetLeveringskvittering();

                //Act

                //Assert
                Assert.AreEqual(leveringskvittering.Generert, leveringskvittering.Levert);
            }
        }
    }
}