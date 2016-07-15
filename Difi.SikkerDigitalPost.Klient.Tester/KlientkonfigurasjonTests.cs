using System;
using System.IO;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class KlientkonfigurasjonTests
    {
        public class KonstruktørMethod : KlientkonfigurasjonTests
        {
            [Fact]
            public void InitializesProperties()
            {
                //Arrange
                var environment = Miljø.FunksjoneltTestmiljø;
                const string organizationNumberPosten = "984661185";
                object proxyhost = null;
                const string proxyScheme = "https";
                var timeoutInMilliseconds = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;
                const int proxyPort = 0;

                //Act
                var clientConfiguration = new Klientkonfigurasjon(environment);

                //Assert
                Assert.Equal(environment, clientConfiguration.Miljø);
                Assert.Equal(organizationNumberPosten, clientConfiguration.MeldingsformidlerOrganisasjon.Verdi);
                Assert.Equal(proxyhost, clientConfiguration.ProxyHost);
                Assert.Equal(proxyScheme, clientConfiguration.ProxyScheme);
                Assert.Equal(timeoutInMilliseconds, clientConfiguration.TimeoutIMillisekunder);
                Assert.Equal(proxyPort, clientConfiguration.ProxyPort);
                Assert.Equal(clientConfiguration.LoggForespørselOgRespons, false);
            }
        }

        public class EnableDocumentBundleDiskDumpMethod : KlientkonfigurasjonTests
        {
            [Fact]
            public void AddsDocumentBundleToDiskProcessor()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                clientConfiguration.AktiverLagringAvDokumentpakkeTilDisk(Path.GetTempPath());

                //Assert
                Assert.True(clientConfiguration.Dokumentpakkeprosessorer.Any(p => p.GetType() == typeof (LagreDokumentpakkeTilDiskProsessor)));
            }

            [Fact]
            public void NonExistingFolderShouldThrowException()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                Assert.Throws<DirectoryNotFoundException>(() =>
                    clientConfiguration.AktiverLagringAvDokumentpakkeTilDisk(@"c:\nonexistentfolder\theoddsofthisexistingis\extremelylow")
                    );
            }

            [Fact]
            public void ExistingFolderShouldNotThrowException()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);
                var path = Path.GetTempPath();

                //Act
                clientConfiguration.AktiverLagringAvDokumentpakkeTilDisk(path);

                //Assert
                Assert.True(clientConfiguration.Dokumentpakkeprosessorer.Any(p => p.GetType() == typeof (LagreDokumentpakkeTilDiskProsessor)));
            }
        }
    }
}