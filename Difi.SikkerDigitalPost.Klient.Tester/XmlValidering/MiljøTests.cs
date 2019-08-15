using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Digipost.Api.Client.Shared.Certificate;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.XmlValidering
{
    public class MiljøTests
    {
        public class FullUriMethod : MiljøTests
        {
            [Fact]
            public void Appends_databehandler_and_avsender_organisasjonsnummer()
            {
                //Arrange
                var miljø = Miljø.Produksjonsmiljø;

                var databehandlerOrgnr = "123456789";
                var avsenderOrgnr = "987654321";

                var databehandlerOrganisasjonsnummer =  new Organisasjonsnummer(databehandlerOrgnr);
                var avsenderOrganisasjonsnummer = new Organisasjonsnummer(avsenderOrgnr);

                //Act
                var fullUri = miljø.UrlWithOrganisasjonsnummer(databehandlerOrganisasjonsnummer, avsenderOrganisasjonsnummer);

                //Assert
                Assert.Contains($"9908:{databehandlerOrgnr}/9908:{avsenderOrgnr}", fullUri.ToString());
            }
        }

        public class GetMiljøMethod : MiljøTests
        {
            [Fact]
            public void Returnerer_initialisert_funksjonelt_testmiljø()
            {
                //Arrange
                var url = "https://qaoffentlig.meldingsformidler.digipost.no/api/";
                var miljø = Miljø.FunksjoneltTestmiljø;
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                Assert.Equal(url, miljø.Url.AbsoluteUri);
                Assert.Equal(sertifikater, miljø.GodkjenteKjedeSertifikater);
            }

            [Fact]
            public void Returnerer_initialisert_produksjonsmiljø()
            {
                //Arrange
                var url = "https://meldingsformidler.digipost.no/api/";
                var miljø = Miljø.Produksjonsmiljø;
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.Equal(url, miljø.Url.ToString());
                Assert.Equal(sertifikater, miljø.GodkjenteKjedeSertifikater);
            }

            [Fact]
            public void Returnerer_initialisert_funksjonelt_testmiljø_norsk_helsenett()
            {
                //Arrange
                var url = "https://qaoffentlig.meldingsformidler.nhn.digipost.no:4445/api/";
                var miljø = Miljø.FunksjoneltTestmiljøNorskHelsenett;
                var sertifikater = CertificateChainUtility.FunksjoneltTestmiljøSertifikater();

                //Act

                //Assert
                Assert.Equal(url, miljø.Url.AbsoluteUri);
                Assert.Equal(sertifikater, miljø.GodkjenteKjedeSertifikater);
            }

            [Fact]
            public void Returnerer_initialisert_produksjonsmiljø_norsk_helsenett()
            {
                //Arrange
                var url = "https://meldingsformidler.nhn.digipost.no:4444/api/";
                var miljø = Miljø.ProduksjonsmiljøNorskHelsenett;
                var sertifikater = CertificateChainUtility.ProduksjonsSertifikater();

                //Act

                //Assert
                Assert.Equal(url, miljø.Url.ToString());
                Assert.Equal(sertifikater, miljø.GodkjenteKjedeSertifikater);
            }
        }
    }
}