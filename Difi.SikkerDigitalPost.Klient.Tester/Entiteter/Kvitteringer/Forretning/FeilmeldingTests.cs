using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning.Tests;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer.Forretning
{
    [TestClass()]
    public class FeilmeldingTests
    {
        [TestClass]
        public class KonstruktørMethod : FeilmeldingTests
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var konversasjonsId = Guid.NewGuid();
                var bodyReferenceUri = "bodyReferenceUri";
                var digestValue = "digestValue";

                //Act
                var feilmelding = new Feilmelding(konversasjonsId, bodyReferenceUri, digestValue);

                //Assert
                Assert.AreEqual(konversasjonsId, feilmelding.KonversasjonsId);
                Assert.AreEqual(bodyReferenceUri, feilmelding.BodyReferenceUri);
                Assert.AreEqual(digestValue, feilmelding.DigestValue);
            }
        }

        [TestClass]
        public class FeiletMethod : MottakskvitteringTests
        {
            [TestMethod]
            public void ReturnererGenerertTidspunkt()
            {
                //Arrange
                var feilmelding = DomeneUtility.GetFeilmelding();

                //Act

                //Assert
                Assert.AreEqual(feilmelding.Generert, feilmelding.Feilet);
            }
        }


    }
}