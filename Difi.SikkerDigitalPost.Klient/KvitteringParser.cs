using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;

namespace Difi.SikkerDigitalPost.Klient
{
    public class KvitteringParser
    {
        public static VarslingFeiletKvittering TilVarslingFeiletKvittering(XmlDocument varslingFeiletXmlDocument)
        {
            throw new NotImplementedException();
        }

        public static Leveringskvittering TilLeveringskvittering(XmlDocument leveringskvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        public static Mottakskvittering TilMottakskvittering(XmlDocument mottakskvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        public static Returpostkvittering TilReturpostkvittering(XmlDocument returpostkvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        public static Åpningskvittering TilÅpningskvittering(XmlDocument åpningskvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }
    }
}
