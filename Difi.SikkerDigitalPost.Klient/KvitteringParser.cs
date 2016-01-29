using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient
{
    public class KvitteringParser
    {

        public static Leveringskvittering TilLeveringskvittering(XmlDocument leveringskvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(leveringskvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(leveringskvitteringXmlDocument);

            return new Leveringskvittering(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public Mottakskvittering TilMottakskvittering(XmlDocument mottakskvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        public Returpostkvittering TilReturpostkvittering(XmlDocument returpostkvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        public VarslingFeiletKvittering TilVarslingFeiletKvittering(XmlDocument varslingFeiletXmlDocument)
        {
            throw new NotImplementedException();
        }

        public Åpningskvittering TilÅpningskvittering(XmlDocument åpningskvitteringXmlDocument)
        {
            throw new NotImplementedException();
        }

        private static Kvitteringsfelter HentKvitteringsfelter(XmlDocument kvittering)
        {
            var kvitteringsfelter = new Kvitteringsfelter();

            try
            {
                kvitteringsfelter.SendtTidspunkt = Convert.ToDateTime(GetXmlNodeFromDocument(kvittering,"//ns6:Timestamp").InnerText);
                kvitteringsfelter.MeldingsId = GetXmlNodeFromDocument(kvittering,"//ns6:MessageId").InnerText;

                var referanseTilMeldingId = GetXmlNodeFromDocument(kvittering,"//ns6:RefToMessageId");
                if (referanseTilMeldingId != null)
                {
                    kvitteringsfelter.ReferanseTilMeldingId = referanseTilMeldingId.InnerText;
                }
                kvitteringsfelter.Rådata = kvittering.OuterXml;
            }
            catch (Exception e)
            {
                throw new XmlParseException(string.Format("Feil under bygging av {0} (av type Kvittering). Klarte ikke finne alle felter i xml.", e.GetType()), e);
            }

            return kvitteringsfelter;
        }

        private static Forretningskvitteringfelter HentForretningskvitteringFelter(XmlDocument forretningskvittering)
        {
            var forretningskvittergFelter = new Forretningskvitteringfelter();

            var bodyId = SjekkForretningskvitteringForKonsistens(forretningskvittering);

            try
            {
                var guidNode = GetXmlNodeFromDocument(forretningskvittering,"//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier");
                forretningskvittergFelter.KonversasjonsId = new Guid(guidNode.InnerText);

                var tidspunktNode = GetXmlNodeFromDocument(forretningskvittering,"//ns9:tidspunkt");
                forretningskvittergFelter.Generert = Convert.ToDateTime(tidspunktNode.InnerText);


                var bodyReferenceNode = forretningskvittering.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']", GetNamespaceManager(forretningskvittering));
                forretningskvittergFelter.BodyReferenceUri = bodyReferenceNode.Attributes["URI"].Value;
                forretningskvittergFelter.DigestValue = bodyReferenceNode.SelectSingleNode("//ds:DigestValue", GetNamespaceManager(forretningskvittering)).InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException(string.Format("Feil under bygging av {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml.", e.GetType()), e);
            }

            return forretningskvittergFelter;
        }

        private static string SjekkForretningskvitteringForKonsistens(XmlDocument document)
        {
            var partInfo = document.SelectSingleNode("//ns6:PartInfo", GetNamespaceManager(document));
            var partInfoBodyId = String.Empty;
            if (partInfo.Attributes.Count > 0)
                partInfoBodyId = partInfo.Attributes["href"].Value;

            string bodyId = document.SelectSingleNode("//env:Body", GetNamespaceManager(document)).Attributes["wsu:Id"].Value;

            if (!partInfoBodyId.Equals(String.Empty) && !bodyId.Equals(partInfoBodyId))
            {
                throw new Exception(
                    String.Format(
                        "Id i PartInfo og i Body matcher er ikke like. Partinfo har '{0}', body har '{1}'",
                        partInfoBodyId,
                        bodyId));
            }
            return bodyId;
        }

        protected static XmlNode GetXmlNodeFromDocument(XmlDocument document, string xPath)
        {
            try
            {
                var rot = document.DocumentElement;
                var targetNode = rot.SelectSingleNode(xPath, GetNamespaceManager(document));

                return targetNode;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    String.Format("Feil under henting av dokumentnode i {0} (av type Forretningskvittering). Klarte ikke finne alle felter i xml."
                    , e.GetType()), e);
            }
        }


        private static XmlNamespaceManager GetNamespaceManager(XmlDocument document)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            manager.AddNamespace("eb", NavneromUtility.EbXmlCore);
            manager.AddNamespace("ns3", NavneromUtility.StandardBusinessDocumentHeader);
            manager.AddNamespace("ns5", NavneromUtility.XmlDsig);
            manager.AddNamespace("ns6", NavneromUtility.EbXmlCore);
            manager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
            manager.AddNamespace("ds", NavneromUtility.XmlDsig);
            return manager;
        }
    }

    internal class Kvitteringsfelter
    {
        public DateTime SendtTidspunkt { get; set; }
        
        public string MeldingsId { get; set; }

        public string ReferanseTilMeldingId { get; set; }

        public string Rådata { get; set; }
    }

    internal class Forretningskvitteringfelter
    {
        public Guid KonversasjonsId { get; set; }

        public string BodyReferenceUri { get; set; }

        public string DigestValue { get; set; }

        public DateTime Generert { get; set; }
    }

}
