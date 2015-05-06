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
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// Abstrakt klasse for forretningskvitteringer.
    /// </summary>
    public abstract class Forretningskvittering : Kvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        /// <summary>
        /// Tidspunktet da kvitteringen ble sendt.
        /// </summary>
        public DateTime Tidspunkt { get; protected set; }

        /// <summary>
        /// Identifiserer en melding og tilhørende kvitteringer unikt.
        /// </summary>
        public Guid KonversasjonsId { get; protected set; }
        
        internal XmlNode BodyReference { get; set; }


        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public override abstract string ToString();

        protected Forretningskvittering() { }

        protected Forretningskvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;

                KonversasjonsId = new Guid(DocumentNode("//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier").InnerText);
                BodyReference = BodyReferenceNode();
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under bygging av {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml."
                    , GetType()), e);
            }
        }

        protected XmlNode BodyReferenceNode()
        {
            XmlNode bodyReferenceNode;

            try
            {
                XmlNode rotnode = _document.DocumentElement;

                var partInfo = rotnode.SelectSingleNode("//ns6:PartInfo", _namespaceManager);
                var partInfoBodyId = String.Empty;
                if (partInfo.Attributes.Count > 0)
                    partInfoBodyId = partInfo.Attributes["href"].Value;

                string bodyId = rotnode.SelectSingleNode("//env:Body", _namespaceManager).Attributes["wsu:Id"].Value;

                if (!partInfoBodyId.Equals(String.Empty) && !bodyId.Equals(partInfoBodyId))
                {
                    throw new Exception(
                        String.Format(
                        "Id i PartInfo og i Body matcher er ikke like. Partinfo har '{0}', body har '{1}'",
                        partInfoBodyId,
                        bodyId));
                }

                bodyReferenceNode = rotnode.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']",
                    _namespaceManager);
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                  String.Format("Feil under henting av referanser i {0} (av type Forretningskvittering). ",
                  GetType()), e);
            }

            return bodyReferenceNode;
        }

    }
}
