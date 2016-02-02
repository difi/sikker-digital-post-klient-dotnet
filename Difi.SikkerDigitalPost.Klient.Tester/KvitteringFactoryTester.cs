using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class KvitteringFactoryTester
    {
        [TestClass]
        public class GetKvitteringMethod : KvitteringFactoryTester
        {
            [TestMethod]
            public void ReturnererFeilmelding()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.FeilmeldingXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(Feilmelding));
            }

            [TestMethod]
            public void ReturnererLeveringskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.LeveringskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(Leveringskvittering));
            }

            [TestMethod]
            public void ReturnererMottakskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.MottakskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(Mottakskvittering));
            }

            [TestMethod]
            public void ReturnererReturpostkvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ReturpostkvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(Returpostkvittering));
            }

            [TestMethod]
            public void ReturnererVarslingFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.VarslingFeiletKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(VarslingFeiletKvittering));
            }

            [TestMethod]
            public void ReturnererÅpningskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ÅpningskvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(Åpningskvittering));
            }

            [TestMethod]
            public void ReturnererTomKøKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TomKøKvitteringXml();

                //Act
                var kvittering = KvitteringFactory.GetKvittering(xml);

                //Assert
                Assert.IsInstanceOfType(kvittering, typeof(TomKøKvittering));
            }


        }
    }
}