using System;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests
{
    [TestClass()]
    public class ReturpostkvitteringTests
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
                var returpostkvittering = new Returpostkvittering(konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, returpostkvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, returpostkvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, returpostkvittering.DigestValue);
            }
        }

        [TestClass]
        public class MottattMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var mottakskvittering = DomeneUtility.GetReturpostkvittering();

                //Act

                //Assert
                Assert.AreEqual(mottakskvittering.Generert, mottakskvittering.Returnert);
            }
        }

    }
}