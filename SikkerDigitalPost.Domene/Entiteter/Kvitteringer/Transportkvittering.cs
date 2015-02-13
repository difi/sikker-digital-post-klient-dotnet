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
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// Abstrakt klasse for transportkvitteringer.
    /// </summary>
    public abstract class Transportkvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        /// <summary>
        /// Tidspunktet da kvitteringen ble sendt.
        /// </summary>
        public DateTime Tidspunkt { get; protected set; }

        /// <summary>
        /// Unik identifikator for kvitteringen.
        /// </summary>
        public string MeldingsId { get; protected set; }

        /// <summary>
        /// Refereranse til en annen relatert melding. Refererer til den relaterte meldingens MessageId.
        /// </summary>
        public string ReferanseTilMeldingsId { get; protected set; }

        /// <summary>
        /// Kvitteringen presentert som tekststreng.
        /// </summary>
        public string Rådata { get; protected set; }

        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public abstract override string ToString();
        protected Transportkvittering()
        { }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;
                Tidspunkt = Convert.ToDateTime(DocumentNode("//ns6:Timestamp").InnerText);
                MeldingsId = DocumentNode("//ns6:MessageId").InnerText;

                var refToMessageIdNode = DocumentNode("//ns6:RefToMessageId");
                if (refToMessageIdNode != null)
                {
                    ReferanseTilMeldingsId = refToMessageIdNode.InnerText;
                }

                Rådata = document.OuterXml;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                   String.Format("Feil under bygging av {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                   , GetType()), e);
            }
        }

        protected XmlNode DocumentNode(string xPath)
        {
            try
            {
                var rot = _document.DocumentElement;
                var targetNode = rot.SelectSingleNode(xPath, _namespaceManager);

                return targetNode;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under henting av dokumentnode i {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                    , GetType()), e);
            }
        }
    }
}
