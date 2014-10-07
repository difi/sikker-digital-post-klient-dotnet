using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.Xml;

namespace SikkerDigitalPost.Klient
{
    internal class SignaturBygger
    {
        private const string XsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/ts_102918v010201.xsd";

        private readonly Signatur _signatur;
        private readonly Forsendelse _forsendelse;
        private readonly Manifest _manifest;

        private XmlDocument _signaturDokumentXml;

        internal SignaturBygger(Signatur signatur, Forsendelse forsendelse, Manifest manifest)
        {
            _signatur = signatur;
            _forsendelse = forsendelse;
            _manifest = manifest;
        }

        public void Bygg()
        {
            _signaturDokumentXml = OpprettXmlDokument();

            var signaturnode = Signaturnode(_signaturDokumentXml, _signatur);

            IEnumerable<IAsiceVedlegg> referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument, _forsendelse.Dokumentpakke.Vedlegg, _manifest);
            OpprettReferanser(signaturnode, referanser);

            var keyInfoX509Data = new KeyInfoX509Data(_signatur.Sertifikat, X509IncludeOption.WholeChain);
            signaturnode.KeyInfo.AddClause(keyInfoX509Data);
            signaturnode.ComputeSignature();

            _signaturDokumentXml.DocumentElement.AppendChild(_signaturDokumentXml.ImportNode(signaturnode.GetXml(), true));

            _signatur.Bytes = Encoding.UTF8.GetBytes(_signaturDokumentXml.OuterXml);
        }

        public void SkrivXmlTilFil(string filsti)
        {
            _signaturDokumentXml.Save(filsti);
        }

        private static Sha256Reference SignedPropertiesReferanse()
        {
            var signedPropertiesReference = new Sha256Reference("#SignedProperties")
            {
                Type = "http://uri.etsi.org/01903#SignedProperties"
            };
            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
            return signedPropertiesReference;
        }

        private void OpprettReferanser(SignedXml signaturnode, IEnumerable<IAsiceVedlegg> referanser)
        {
            foreach (var item in referanser)
            {
                signaturnode.AddReference(Sha256Referanse(item));
            }

            signaturnode.AddObject(
                new QualifyingPropertiesObject(
                    _signatur.Sertifikat, "#Signature", referanser.ToArray(), _signaturDokumentXml.DocumentElement)
                    );

            signaturnode.AddReference(SignedPropertiesReferanse());
        }

        private static IEnumerable<IAsiceVedlegg> Referanser(Dokument hoveddokument, IEnumerable<IAsiceVedlegg> vedlegg, Manifest manifest)
        {
            var referanser = new List<IAsiceVedlegg>();
            referanser.Add(hoveddokument);
            referanser.AddRange(vedlegg);
            referanser.Add(manifest);
            return referanser;
        }

        private static SignedXml Signaturnode(XmlDocument signaturXml, Signatur signatur)
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(signaturXml, signatur.Sertifikat);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private XmlDocument OpprettXmlDokument()
        {
            var signaturXml = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            signaturXml.AppendChild(signaturXml.CreateElement("xades", "XAdESSignatures", Navnerom.Ns10));
            signaturXml.DocumentElement.SetAttribute("xmlns:xsi", Navnerom.xsi);
            signaturXml.DocumentElement.SetAttribute("schemaLocation", Navnerom.xsi, XsiSchemaLocation);
            signaturXml.DocumentElement.SetAttribute("xmlns:ns11", Navnerom.Ns11);


            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
            return signaturXml;
        }

        private Sha256Reference Sha256Referanse(IAsiceVedlegg dokument)
        {
            return new Sha256Reference(dokument.Bytes)
            {
                Uri = dokument.Filnavn,
                Id = dokument.Filnavn
            };
        }
    }
}
