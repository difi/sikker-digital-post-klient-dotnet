using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klientbibliotek.Envelope;
using SikkerDigitalPost.Klientbibliotek.Xml;

namespace SikkerDigitalPost.Klientbibliotek
{
    internal class SignaturBygger
    {
        private const string XsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/ts_102918v010201.xsd";

        private readonly Signatur _signatur;
        private readonly Forsendelse _forsendelse;
        private XmlDocument _signaturDokumentXml;

        internal SignaturBygger(Signatur signatur, Forsendelse forsendelse)
        {
            _signatur = signatur;
            _forsendelse = forsendelse;
        }

        public void Bygg()
        {
            _signaturDokumentXml = OpprettXmlDokument();

            var signaturnode = Signaturnode(_signaturDokumentXml, _signatur);

            IEnumerable<Dokument> referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument, _forsendelse.Dokumentpakke.Vedlegg);
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

        private void OpprettReferanser(SignedXml signaturnode, IEnumerable<Dokument> referanser)
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

        private static IEnumerable<Dokument> Referanser(Dokument hoveddokument, IEnumerable<Dokument> vedlegg)
        {
            var referanser = new List<Dokument>();
            referanser.Add(hoveddokument);
            referanser.AddRange(vedlegg);
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
            signaturXml.AppendChild(signaturXml.CreateElement("XAdESSignatures", Navnerom.Ns10));
            signaturXml.DocumentElement.SetAttribute("xmlns:xsi", Navnerom.xsi);
            signaturXml.DocumentElement.SetAttribute("schemaLocation", Navnerom.xsi, XsiSchemaLocation);
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
