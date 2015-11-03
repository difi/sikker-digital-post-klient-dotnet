using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering.Tests
{
    [TestClass]
    public class SertifikatkjedevalidatorFunksjoneltTestmiljøTester
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata.sertifikater");

        [TestClass]
        public class ErGyldigResponssertifikatMethod : SertifikatkjedevalidatorFunksjoneltTestmiljøTester
        {
            [TestMethod]
            public void GodkjennerTestsertifikat()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakersertifikatFraOppslagstjenesten.pem"));

                //Act
                var sertifikatValidator = new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void ErGyldigSertifikatOgKjedestatus()
            {
                //Arrange
                var testSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "test", "testmottakersertifikatFraOppslagstjenesten.pem"));
                X509ChainStatus[] kjedestatus;

                //Act
                var sertifikatValidator = new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(testSertifikat, out kjedestatus);

                //Assert
                Assert.IsTrue(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.Count() == 0 || kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

            [TestMethod] public void FeilerMedSertifikatUtenGyldigKjede()
            {
                //Arrange
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "enhetstester", "difi-enhetstester.cer"));

                //Act
                var sertifikatValidator = new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());

                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(selvsignertSertifikat);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
            }

            [TestMethod]
            public void FeilerMedSertifikatUtenGyldigKjedeReturnererKjedestatus()
            {
                var selvsignertSertifikat = new X509Certificate2(ResourceUtility.ReadAllBytes(true, "enhetstester", "difi-enhetstester.cer"));
                //Act
                var sertifikatValidator = new SertifikatkjedevalidatorFunksjoneltTestmiljø(SertifikatkjedeUtility.FunksjoneltTestmiljøSertifikater());

                X509ChainStatus[] kjedestatus;
                var erGyldigResponssertifikat = sertifikatValidator.ErGyldigResponssertifikat(selvsignertSertifikat, out kjedestatus);

                //Assert
                Assert.IsFalse(erGyldigResponssertifikat);
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);

            }
        }
    }
}