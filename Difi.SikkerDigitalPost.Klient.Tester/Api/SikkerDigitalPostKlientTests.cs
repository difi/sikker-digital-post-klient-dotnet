using System;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Api
{
    [TestClass()]
    public class SikkerDigitalPostKlientTests
    {
        [TestClass]
        public class ConstructorMethod : SikkerDigitalPostKlientTests
        {
            [TestMethod]
            public void InitializesFields()
            {
                //Arrange
                var databehandler = new Databehandler(new Organisasjonsnummer("999999999"), DomeneUtility.GetAvsenderSertifikat());
                var klientkonfigurasjon = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                var sikkerDigitalPostKlient = new  SikkerDigitalPostKlient(databehandler, klientkonfigurasjon);

                //Assert
                Assert.AreEqual(klientkonfigurasjon, sikkerDigitalPostKlient.Klientkonfigurasjon);
                Assert.AreEqual(databehandler, sikkerDigitalPostKlient.Databehandler);
                Assert.IsInstanceOfType(sikkerDigitalPostKlient.RequestHelper, typeof(RequestHelper));
            } 
        }
    }
}