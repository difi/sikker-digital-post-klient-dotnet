using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public readonly DateTime Åpningstidspunkt;

        internal Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            try
            {
                Åpningstidspunkt = Convert.ToDateTime(DocumentNode("ns9:tidspunkt").InnerText);
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Åpningskvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }
    }
}
