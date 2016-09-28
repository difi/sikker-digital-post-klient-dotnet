using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.XmlValidering
{
    public class MiljøTester
    {
        public class GetMiljøMethod : MiljøTester
        {
            [Fact]
            public void ReturnererInitialisertFunksjoneltTestmiljø()
            {
                //Arrange
                var url = "https://qaoffentlig.meldingsformidler.digipost.no/api/ebms";
                var miljø = Miljø.FunksjoneltTestmiljø;
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                Assert.NotNull(miljø.CertificateChainValidator);
                Assert.Equal(url, miljø.Url.AbsoluteUri);
                Assert.Equal(sertifikater, miljø.CertificateChainValidator.CertificateStore);
            }

            [Fact]
            public void ReturnererInitialisertProduksjonsmiljø()
            {
                //Arrange
                var url = "https://meldingsformidler.digipost.no/api/ebms";
                var miljø = Miljø.Produksjonsmiljø;
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.NotNull(miljø.CertificateChainValidator);
                Assert.Equal(url, miljø.Url.ToString());
                Assert.Equal(sertifikater, miljø.CertificateChainValidator.CertificateStore);
            }

            [Fact]
            public void ReturnererInitialisertFunksjoneltTestmiljøNorskHelsenett()
            {
                //Arrange
                var url = "https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/";
                var miljø = Miljø.FunksjoneltTestmiljøNorskHelsenett;
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                Assert.NotNull(miljø.CertificateChainValidator);
                Assert.Equal(url, miljø.Url.AbsoluteUri);
                Assert.Equal(sertifikater, miljø.CertificateChainValidator.CertificateStore);
            }

            [Fact]
            public void ReturnererInitialisertProduksjonsmiljøNorskHelsenett()
            {
                //Arrange
                var url = "https://meldingsformidler.nhn.digipost.no:4444/api/";
                var miljø = Miljø.ProduksjonsmiljøNorskHelsenett;
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.NotNull(miljø.CertificateChainValidator);
                Assert.Equal(url, miljø.Url.ToString());
                Assert.Equal(sertifikater, miljø.CertificateChainValidator.CertificateStore);
            }
        }
    }
}