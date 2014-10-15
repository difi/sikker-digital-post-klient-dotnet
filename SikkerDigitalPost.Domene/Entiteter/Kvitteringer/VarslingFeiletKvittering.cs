using System.Xml;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        public readonly Varslingskanal Varslingskanal;

        public string Beskrivelse;

        internal VarslingFeiletKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
            var varslingskanal = DocumentNode("//ns9:varslingskanal").InnerText;
            Varslingskanal = varslingskanal == Varslingskanal.Epost.ToString()
                ? Varslingskanal.Epost
                : Varslingskanal.Sms;

            var beskrivelseNode = DocumentNode("//ns9:beskrivelse");
            if(beskrivelseNode != null)
                Beskrivelse = beskrivelseNode.InnerText;
        }
    }
}
