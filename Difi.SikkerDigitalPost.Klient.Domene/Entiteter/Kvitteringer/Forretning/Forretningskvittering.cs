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
        /// Identifiserer en melding og tilhørende kvitteringer unikt.
        /// </summary>
        public Guid KonversasjonsId { get; protected set; }
        
        internal string BodyReferenceUri { get; set; }

        internal string DigestValue { get; set; }

        protected DateTime Generert { get; set; }


        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public override abstract string ToString();

        protected Forretningskvittering() { }

        public Forretningskvittering(Guid konversasjonsId, string bodyReferenceUri, string digestValue)
        {
            KonversasjonsId = konversasjonsId;
            BodyReferenceUri = bodyReferenceUri;
            DigestValue = digestValue;
        }

        protected Forretningskvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;

                KonversasjonsId = new Guid(DocumentNode("//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier").InnerText);
                Generert = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);
                SetBodyReferenceUriAndDigest();
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under bygging av {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml."
                    , GetType()), e);
            }
        }

        protected XmlNode SetBodyReferenceUriAndDigest()
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

                bodyReferenceNode = rotnode.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']", _namespaceManager);
                BodyReferenceUri = bodyReferenceNode.Attributes["URI"].Value;

                DigestValue = bodyReferenceNode.SelectSingleNode("//ds:DigestValue", _namespaceManager).InnerText;

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
