using System;
using System.Reflection;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Digipost.Api.Client.Shared.Resources.Resource;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class ManifestTester
    {
        public class KonstruktørMethod : ManifestTester
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                const string id = "Id_1";
                const string mimeType = "application/xml";
                const string filnavn = "manifest.xml";

                var forsendelse = DomainUtility.GetForsendelseWithTestCertificate();
                var manifest = new Manifest(forsendelse);

                //Act

                //Assert
                Assert.Equal(forsendelse, manifest.Forsendelse);
                Assert.Equal(forsendelse.Avsender, manifest.Avsender);
                Assert.Equal(id, manifest.Id);
                Assert.Equal(mimeType, manifest.MimeType);
                Assert.Equal(filnavn, manifest.Filnavn);
            }
        }

        public class Hoveddokument : ManifestTester
        {
            [Fact]
            public void UgyldigNavnPåHoveddokumentValidererIkke()
            {
                var manifest = new Manifest(DomainUtility.GetForsendelseWithTestCertificate());

                var manifestXml = manifest.Xml();
                var manifestValidator = SdpXmlValidator.Instance;

                var namespaceManager = new XmlNamespaceManager(manifestXml.NameTable);
                namespaceManager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
                namespaceManager.AddNamespace("ds", NavneromUtility.XmlDsig);

                var hoveddokumentNode = manifestXml.DocumentElement.SelectSingleNode("//ns9:hoveddokument",
                    namespaceManager);
                var gammelVerdi = hoveddokumentNode.Attributes["href"].Value;
                hoveddokumentNode.Attributes["href"].Value = "abc"; //Endre navn på hoveddokument til å være for kort

                string validationMessages;
                var validert = manifestValidator.Validate(manifestXml.OuterXml, out validationMessages);
                Assert.False(validert, validationMessages);

                hoveddokumentNode.Attributes["href"].Value = gammelVerdi;
            }
        }

        public class Vedlegg : ManifestTester
        {
            [Fact]
            public void VedleggTittelSkalSettesIManifestet()
            {
                //Arrange
                var resourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "testdata");
                var dokument = new Dokument("hoved", resourceUtility.ReadAllBytes("hoveddokument", "Hoveddokument.pdf"), "application/pdf");
                var vedleggTittel = "tittel";
                var vedlegg = new Dokument(vedleggTittel, resourceUtility.ReadAllBytes("hoveddokument", "Hoveddokument.pdf"),
                    "application/pdf");

                var dokumentPakke = new Dokumentpakke(dokument);
                dokumentPakke.LeggTilVedlegg(vedlegg);

                var message = new Forsendelse(DomainUtility.GetAvsender(), DomainUtility.GetDigitalPostInfoSimple(), dokumentPakke, Prioritet.Normal, Guid.NewGuid().ToString());
                var asiceArkiv = DomainUtility.GetAsiceArchive(message);

                var manifestXml = new Manifest(message).Xml();
                var namespaceManager = new XmlNamespaceManager(manifestXml.NameTable);
                namespaceManager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
                namespaceManager.AddNamespace("ds", NavneromUtility.XmlDsig);
                //Act

                //Assert

                var vedleggNodeInnerText = manifestXml.DocumentElement.SelectSingleNode("//ns9:vedlegg", namespaceManager).InnerText;
                Assert.Equal(vedleggTittel, vedleggNodeInnerText);
            }

            [Fact]
            public void HoveddokumentTittelSkalSettesIManifestet()
            {
                //Arrange
                var resourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "testdata");
                const string hoveddokumentTittel = "hoveddokument tittel";
                var dokument = new Dokument(hoveddokumentTittel, resourceUtility.ReadAllBytes("hoveddokument", "Hoveddokument.pdf"), "application/pdf");

                var vedlegg = new Dokument("vedlegg tittel", resourceUtility.ReadAllBytes("hoveddokument", "Hoveddokument.pdf"),
                    "application/pdf");

                var dokumentPakke = new Dokumentpakke(dokument);
                dokumentPakke.LeggTilVedlegg(vedlegg);

                var message = new Forsendelse(DomainUtility.GetAvsender(), DomainUtility.GetDigitalPostInfoWithTestCertificate(), dokumentPakke, Prioritet.Normal, Guid.NewGuid().ToString());

                var manifestXml = new Manifest(message).Xml();
                var namespaceManager = new XmlNamespaceManager(manifestXml.NameTable);
                namespaceManager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
                namespaceManager.AddNamespace("ds", NavneromUtility.XmlDsig);
                //Act

                //Assert
                var vedleggNodeInnerText = manifestXml.DocumentElement.SelectSingleNode("//ns9:hoveddokument",
                    namespaceManager).InnerText;
                Assert.Equal(hoveddokumentTittel, vedleggNodeInnerText);
            }
        }

        public class XsdValidering
        {
            [Fact]
            public void ValidereManifestMotXsdValiderer()
            {
                var message = DomainUtility.GetForsendelseWithTestCertificate();
                var arkiv = DomainUtility.GetAsiceArchive(message);

                var manifestXml = new Manifest(message).Xml();

                string validationMessages;
                var validert = SdpXmlValidator.Instance.Validate(manifestXml.OuterXml, out validationMessages);
                Assert.True(validert, validationMessages);
            }
        }
    }
}