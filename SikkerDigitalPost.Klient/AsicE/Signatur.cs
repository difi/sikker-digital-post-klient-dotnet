/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Security;
using Sha256Reference = Difi.SikkerDigitalPost.Klient.Domene.Sha256Reference;

namespace Difi.SikkerDigitalPost.Klient.AsicE
{
    internal class Signatur : IAsiceVedlegg
    {
        private readonly Forsendelse _forsendelse;
        private readonly Manifest _manifest;
        private readonly X509Certificate2 _sertifikat;
        private XmlDocument _xml;

        public Signatur(Forsendelse forsendelse, Manifest manifest, X509Certificate2 sertifikat)
        {
            _forsendelse = forsendelse;
            _manifest = manifest;
            _sertifikat = sertifikat;
        }

        public string Filnavn
        {
            get { return "META-INF/signatures.xml"; }
        }

        public byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Xml().OuterXml);
            }
        }

        public string Innholdstype
        {
            get { return "application/xml"; }
        }

        public string Id
        {
            get { return "Id_0"; }
        }

        public XmlDocument Xml()
        {
            if (_xml != null)
            {
                return _xml;
            }

            try
            {
                _xml = OpprettXmlDokument();

                var signaturnode = Signaturnode();

                IEnumerable<IAsiceVedlegg> referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument,
                    _forsendelse.Dokumentpakke.Vedlegg, _manifest);
                OpprettReferanser(signaturnode, referanser);

                var keyInfoX509Data = new KeyInfoX509Data(_sertifikat, X509IncludeOption.WholeChain);
                signaturnode.KeyInfo.AddClause(keyInfoX509Data);
                signaturnode.ComputeSignature();

                _xml.DocumentElement.AppendChild(_xml.ImportNode(signaturnode.GetXml(), true));
            }
            catch (Exception e)
            {
                throw new XmlParseException("Kunne ikke bygge Xml for signatur.", e);
            }

            Logging.Log(TraceEventType.Verbose, _forsendelse.KonversasjonsId, "Generert signatur for dokumentpakke" + Environment.NewLine + _xml.OuterXml);

            return _xml;
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
                    _sertifikat, "#Signature", referanser.ToArray(), _xml.DocumentElement)
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

        private SignedXml Signaturnode()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(_xml, _sertifikat);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private XmlDocument OpprettXmlDokument()
        {
            var signaturXml = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            signaturXml.AppendChild(signaturXml.CreateElement("xades", "XAdESSignatures", Navnerom.UriEtsi121));
            signaturXml.DocumentElement.SetAttribute("xmlns:ns11", Navnerom.UriEtsi132);

            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
            return signaturXml;
        }

        private Sha256Reference Sha256Referanse(IAsiceVedlegg dokument)
        {
            return new Sha256Reference(dokument.Bytes)
            {
                Uri = dokument.Filnavn,
                Id = dokument.Id
            };
        }
    }
}
