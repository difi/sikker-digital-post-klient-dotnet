using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
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
                Leveringskvittering leveringskvittering = new Leveringskvittering(konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, leveringskvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, leveringskvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, leveringskvittering.DigestValue);
            }
        }
    }
}