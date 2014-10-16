using System;
using System.Xml;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        public readonly Varslingskanal Varslingskanal;

        public string Beskrivelse;

        internal VarslingFeiletKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
            try
            {
                var varslingskanal = DocumentNode("//ns9:varslingskanal").InnerText;
                Varslingskanal = varslingskanal == Varslingskanal.Epost.ToString()
                    ? Varslingskanal.Epost
                    : Varslingskanal.Sms;

                var beskrivelseNode = DocumentNode("//ns9:beskrivelse");
                if (beskrivelseNode != null)
                    Beskrivelse = beskrivelseNode.InnerText;
            } catch (Exception e)
            {
                throw new XmlParseException(
                    "Feil under bygging av VarslingFeilet-kvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }
    }
}
