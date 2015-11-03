﻿using System.Linq;
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
                Assert.IsTrue(kjedestatus.ElementAt(0).Status == X509ChainStatusFlags.UntrustedRoot);
            }

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
        }
    }
}