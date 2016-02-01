using System;
using System.CodeDom;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Kvitteringsparser
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

        public static Mottakskvittering TilMottakskvittering(XmlDocument mottakskvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(mottakskvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(mottakskvitteringXmlDocument);

            return new Mottakskvittering(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };

        }

        public static Returpostkvittering TilReturpostkvittering(XmlDocument returpostkvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(returpostkvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(returpostkvitteringXmlDocument);

            return new Returpostkvittering(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };

        }

        public static VarslingFeiletKvittering TilVarslingFeiletKvittering(XmlDocument varslingFeiletXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(varslingFeiletXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(varslingFeiletXmlDocument);
            var varslingfeiletKvitteringsfelter = HentVarslingFeiletKvitteringsfelter(varslingFeiletXmlDocument);

            return new VarslingFeiletKvittering(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Beskrivelse = varslingfeiletKvitteringsfelter.Beskrivelse,
                Varslingskanal = varslingfeiletKvitteringsfelter.Varslingskanal
            };
        }

        public static Åpningskvittering TilÅpningskvittering(XmlDocument åpningskvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(åpningskvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(åpningskvitteringXmlDocument);

            return new Åpningskvittering(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Feilmelding TilFeilmelding(XmlDocument feilmelding)
        {
            var kvitteringFelter = HentKvitteringsfelter(feilmelding);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(feilmelding);
            var feilmeldingfelter = HentFeilmeldingsfelter(feilmelding);

            return new Feilmelding(forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Rådata = kvitteringFelter.Rådata,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Skyldig = feilmeldingfelter.SkyldigFeiltype,
                Detaljer = feilmeldingfelter.Detaljer
            };
        }

        private static Kvitteringsfelter HentKvitteringsfelter(XmlDocument kvittering)
        {
            var kvitteringsfelter = new Kvitteringsfelter();

            try
            {
                kvitteringsfelter.SendtTidspunkt = Convert.ToDateTime(GetXmlNodeFromDocument(kvittering,"//ns6:Timestamp").InnerText);
                kvitteringsfelter.MeldingsId = GetXmlNodeFromDocument(kvittering,"//ns6:MessageId").InnerText;
                kvitteringsfelter.ReferanseTilMeldingId = GetXmlNodeFromDocument(kvittering, "//ns6:RefToMessageId").InnerText;
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
            var partInfoBodyId = string.Empty;
            if (partInfo.Attributes.Count > 0)
                partInfoBodyId = partInfo.Attributes["href"].Value;

            string bodyId = document.SelectSingleNode("//env:Body", GetNamespaceManager(document)).Attributes["wsu:Id"].Value;

            if (!partInfoBodyId.Equals(string.Empty) && !bodyId.Equals(partInfoBodyId))
            {
                throw new SdpSecurityException(
                    string.Format("Id i PartInfo og i Body matcher er ikke like. Partinfo har '{0}', body har '{1}'",partInfoBodyId,bodyId));
            }
            return bodyId;
        }

        private static VarslingFeiletKvitteringsfelter HentVarslingFeiletKvitteringsfelter(XmlDocument varslingFeiletKvittering)
        {
            var varslingFeiletKvitteringsfelter = new VarslingFeiletKvitteringsfelter();

            try
            {
                var varslingskanal = GetXmlNodeFromDocument(varslingFeiletKvittering, "//ns9:varslingskanal").InnerText;
                varslingFeiletKvitteringsfelter.Varslingskanal = varslingskanal == Varslingskanal.Epost.ToString()
                    ? Varslingskanal.Epost
                    : Varslingskanal.Sms;

                var beskrivelseNode = GetXmlNodeFromDocument(varslingFeiletKvittering, "//ns9:beskrivelse");
                if (beskrivelseNode != null)
                    varslingFeiletKvitteringsfelter.Beskrivelse = beskrivelseNode.InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    "Feil under bygging av VarslingFeilet-kvittering. Klarte ikke finne alle felter i xml.", e);
            }

            return varslingFeiletKvitteringsfelter;
        }

        private static Feilmeldingsfelter HentFeilmeldingsfelter(XmlDocument feilmelding)
        {
            var feilmeldingsfelter = new Feilmeldingsfelter();

            try
            {
                var feiltype = GetXmlNodeFromDocument(feilmelding, "//ns9:feiltype").InnerText;
                feilmeldingsfelter.SkyldigFeiltype = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower())
                    ? Feiltype.Klient
                    : Feiltype.Server;

                feilmeldingsfelter.Detaljer = GetXmlNodeFromDocument(feilmelding, "//ns9:detaljer").InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Feilmelding-kvittering. Klarte ikke finne alle felter i xml.", e);
            }

            return feilmeldingsfelter;
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

        internal class VarslingFeiletKvitteringsfelter
        {
            public Varslingskanal Varslingskanal { get; set; }

            public string Beskrivelse { get; set; }
        }

        internal class Feilmeldingsfelter
        {
            public Feiltype SkyldigFeiltype { get; set; }

            public string Detaljer { get; set; }

        }
    }
}
