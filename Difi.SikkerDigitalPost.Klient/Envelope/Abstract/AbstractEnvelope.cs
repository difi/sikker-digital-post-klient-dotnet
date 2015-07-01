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
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Abstract
{
    internal abstract class AbstractEnvelope : ISoapVedlegg
    {
        protected readonly XmlDocument EnvelopeXml;
        protected readonly EnvelopeSettings Settings;
        protected AbstractHeader Header;

        private bool _isXmlGenerated = false;
        private byte[] _bytes;
        private string _contentId;

        protected AbstractEnvelope(EnvelopeSettings settings)
        {
            Settings = settings;
            EnvelopeXml = LagXmlRotnode();
        }

        public string Filnavn
        {
            get { return "envelope.xml"; }
        }

        public byte[] Bytes
        {
            get { return _bytes ?? (_bytes = Encoding.UTF8.GetBytes(Xml().OuterXml)); }
        }

        public string Innholdstype
        {
            get { return "application/soap+xml; charset=UTF-8"; }
        }

        public string ContentId
        {
            get { return _contentId ?? (_contentId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid())); }
        }

        public string TransferEncoding
        {
            get { return "binary"; }
        }

        private XmlDocument LagXmlRotnode()
        {
            var xmlDokument = new XmlDocument();
            xmlDokument.PreserveWhitespace = true;
            var xmlDeclaration = xmlDokument.CreateXmlDeclaration("1.0", "UTF-8", null);
            var baseNode = xmlDokument.CreateElement("env", "Envelope", NavneromUtility.SoapEnvelopeEnv12);
            xmlDokument.AppendChild(baseNode);
            xmlDokument.InsertBefore(xmlDeclaration, xmlDokument.DocumentElement);
            return xmlDokument;
        }

        public XmlDocument Xml()
        {
            if (_isXmlGenerated) return EnvelopeXml;

            try
            {
                EnvelopeXml.DocumentElement.AppendChild(HeaderElement());
                EnvelopeXml.DocumentElement.AppendChild(BodyElement());
                Header.AddSignatureElement();
                _isXmlGenerated = true;
            }
            catch (Exception e)
            {
                throw new XmlParseException(String.Format("Kunne ikke bygge Xml for {0} (av type AbstractEnvelope).", GetType()), e);
            }

            return EnvelopeXml;
        }

        protected abstract XmlNode HeaderElement();
        protected abstract XmlNode BodyElement();
    }
}
