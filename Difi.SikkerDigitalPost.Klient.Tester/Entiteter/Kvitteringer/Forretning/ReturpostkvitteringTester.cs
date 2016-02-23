using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    [TestClass]
    public class ReturpostkvitteringTests
    {
        [TestClass]
        public class KonstruktørMethod : ReturpostkvitteringTests
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
                var returpostkvittering = new Returpostkvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, returpostkvittering.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, returpostkvittering.BodyReferenceUri);
                Assert.AreEqual(digestValue, returpostkvittering.DigestValue);
            }
        }

        [TestClass]
        public class ReturnertMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var returpostkvittering = DomeneUtility.GetReturpostkvittering();

                //Act

                //Assert
                Assert.AreEqual(returpostkvittering.Generert, returpostkvittering.Returnert);
            }
        }
    }
}