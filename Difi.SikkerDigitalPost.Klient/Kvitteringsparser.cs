using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Kvitteringsparser
    {
        public static Leveringskvittering TilLeveringskvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);

            return new Leveringskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Mottakskvittering TilMottakskvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);

            return new Mottakskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Returpostkvittering TilReturpostkvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);

            return new Returpostkvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static VarslingFeiletKvittering TilVarslingFeiletKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);
            var varslingfeiletKvitteringsfelter = HentVarslingFeiletKvitteringsfelter(integrasjonspunktKvittering);

            return new VarslingFeiletKvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Beskrivelse = varslingfeiletKvitteringsfelter.Beskrivelse,
                Varslingskanal = varslingfeiletKvitteringsfelter.Varslingskanal
            };
        }

        public static Åpningskvittering TilÅpningskvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);

            return new Åpningskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Feilmelding TilFeilmelding(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(integrasjonspunktKvittering);
            var feilmeldingfelter = HentFeilmeldingsfelter(integrasjonspunktKvittering);

            return new Feilmelding(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Skyldig = feilmeldingfelter.SkyldigFeiltype,
                Detaljer = feilmeldingfelter.Detaljer
            };
        }

        public static TomKøKvittering TilTomKøKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering);

            return new TomKøKvittering
            {
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Xml = kvitteringFelter.Xml
            };
        }

        public static TransportFeiletKvittering TilTransportFeiletKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringFelter = HentKvitteringsfelter(integrasjonspunktKvittering, false);
            var transportFeiletFelter = HentTransportFeiletKvitteringsfelter(integrasjonspunktKvittering);

            return new TransportFeiletKvittering
            {
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Xml = kvitteringFelter.Xml,
                Alvorlighetsgrad = transportFeiletFelter.Alvorlighetsgrad,
                Beskrivelse = transportFeiletFelter.Beskrivelse,
                Feilkode = transportFeiletFelter.Feilkode,
                Kategori = transportFeiletFelter.Kategori,
                Opprinnelse = transportFeiletFelter.Opprinnelse
            };
        }

        private static Kvitteringsfelter HentKvitteringsfelter(IntegrasjonspunktKvittering integrasjonspunktKvittering, bool sjekkEtterReferanseTilMeldingsId = true)
        {
            try
            {
                return ParseKvitteringsFelter(integrasjonspunktKvittering, sjekkEtterReferanseTilMeldingsId);
            }
            catch (Exception e)
            {
                throw new XmlParseException($"Feil under bygging av {e.GetType()} (av type Kvittering).", e);
            }
        }

        private static Kvitteringsfelter ParseKvitteringsFelter(IntegrasjonspunktKvittering integrasjonspunktKvittering, bool sjekkEtterReferanseTilMeldingsId)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            return new Kvitteringsfelter
            {
                SendtTidspunkt = Convert.ToDateTime(GetXmlNodeFromDocument(document, "//ns9:tidspunkt").InnerText),
                MeldingsId = integrasjonspunktKvittering.messageId.ToString(),
                Xml = document,
                ReferanseTilMeldingId = sjekkEtterReferanseTilMeldingsId ? integrasjonspunktKvittering.conversationId.ToString() : null
            };
        }

        public static TransportOkKvittering TilTransportOkKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvitteringsfelter = HentKvitteringsfelter(integrasjonspunktKvittering);

            return new TransportOkKvittering
            {
                MeldingsId = kvitteringsfelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringsfelter.ReferanseTilMeldingId,
                SendtTidspunkt = kvitteringsfelter.SendtTidspunkt,
                Xml = kvitteringsfelter.Xml
            };
        }

        private static Forretningskvitteringfelter HentForretningskvitteringFelter(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var forretningskvittergFelter = new Forretningskvitteringfelter();

            //var bodyId = SjekkForretningskvitteringForKonsistens(integrasjonspunktKvittering);

            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            try
            {
                var guidNode = GetXmlNodeFromDocument(document, "//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier");
                forretningskvittergFelter.KonversasjonsId = new Guid(guidNode.InnerText);

                var tidspunktNode = GetXmlNodeFromDocument(document, "//ns9:tidspunkt");
                forretningskvittergFelter.Generert = Convert.ToDateTime(tidspunktNode.InnerText);

                var bodyReferenceNode = document.SelectSingleNode("//ns9:kvittering/ns5:Signature/ns5:SignedInfo/ns5:Reference", GetNamespaceManager(document));
                forretningskvittergFelter.BodyReferenceUri = bodyReferenceNode.Attributes["URI"].Value;
                forretningskvittergFelter.DigestValue = bodyReferenceNode.SelectSingleNode("//ds:DigestValue", GetNamespaceManager(document)).InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException($"Feil under bygging av {e.GetType()} (av type Forretningskvittering).", e);
            }

            return forretningskvittergFelter;
        }

        private static string SjekkForretningskvitteringForKonsistens(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            var partInfo = document.SelectSingleNode("//ns6:PartInfo", GetNamespaceManager(document));
            var partInfoBodyId = string.Empty;
            if (partInfo.Attributes.Count > 0)
                partInfoBodyId = partInfo.Attributes["href"].Value;

            var bodyId = document.SelectSingleNode("//env:Body", GetNamespaceManager(document)).Attributes["wsu:Id"].Value;

            if (!partInfoBodyId.Equals(string.Empty) && !bodyId.Equals(partInfoBodyId))
            {
                throw new SecurityException(
                    $"Id i PartInfo og i Body matcher er ikke like. Partinfo har '{partInfoBodyId}', body har '{bodyId}'");
            }
            return bodyId;
        }

        private static VarslingFeiletKvitteringsfelter HentVarslingFeiletKvitteringsfelter(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var varslingFeiletKvitteringsfelter = new VarslingFeiletKvitteringsfelter();

            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            try
            {
                var varslingskanal = GetXmlNodeFromDocument(document, "//ns9:varslingskanal").InnerText;
                varslingFeiletKvitteringsfelter.Varslingskanal = varslingskanal == Varslingskanal.Epost.ToString()
                    ? Varslingskanal.Epost
                    : Varslingskanal.Sms;

                var beskrivelseNode = GetXmlNodeFromDocument(document, "//ns9:beskrivelse");
                if (beskrivelseNode != null)
                    varslingFeiletKvitteringsfelter.Beskrivelse = beskrivelseNode.InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    "Feil under bygging av VarslingFeilet-kvittering.", e);
            }

            return varslingFeiletKvitteringsfelter;
        }

        private static Feilmeldingsfelter HentFeilmeldingsfelter(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var feilmeldingsfelter = new Feilmeldingsfelter();

            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            try
            {
                var feiltype = GetXmlNodeFromDocument(document, "//ns9:feiltype").InnerText;
                feilmeldingsfelter.SkyldigFeiltype = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower())
                    ? Feiltype.Klient
                    : Feiltype.Server;

                feilmeldingsfelter.Detaljer = GetXmlNodeFromDocument(document, "//ns9:detaljer").InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Feilmelding-kvittering.", e);
            }

            return feilmeldingsfelter;
        }

        private static TransportFeiletKvitteringsfelter HentTransportFeiletKvitteringsfelter(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var transportFeiletKvitteringsfelter = new TransportFeiletKvitteringsfelter();

            XmlDocument document = new XmlDocument();
            document.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            try
            {
                var errorNode = GetXmlNodeFromDocument(document, "//ns6:Error");
                transportFeiletKvitteringsfelter.Kategori = errorNode.Attributes["category"].Value;
                transportFeiletKvitteringsfelter.Feilkode = errorNode.Attributes["errorCode"].Value;
                transportFeiletKvitteringsfelter.Opprinnelse = errorNode.Attributes["origin"].Value;
                transportFeiletKvitteringsfelter.Alvorlighetsgrad = errorNode.Attributes["severity"].Value;
                transportFeiletKvitteringsfelter.Beskrivelse = GetXmlNodeFromDocument(document, "//ns6:Description").InnerText;
                var skyldig = GetXmlNodeFromDocument(document, "//env:Value").InnerText;
                transportFeiletKvitteringsfelter.SkyldigFeiltype = skyldig == Feiltype.Klient.ToString()
                    ? Feiltype.Klient
                    : Feiltype.Server;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    "Feil under bygging av TransportFeilet-kvittering.", e);
            }

            return transportFeiletKvitteringsfelter;
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
                    $"Feil under henting av dokumentnode i {e.GetType()} (av type Forretningskvittering). Klarte ikke finne alle felter i xml.", e);
            }
        }

        private static XmlNamespaceManager GetNamespaceManager(XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
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

            public XmlDocument Xml { get; set; }
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

        internal class TransportFeiletKvitteringsfelter
        {
            public string Feilkode { get; set; }

            public string Kategori { get; set; }

            public string Opprinnelse { get; set; }

            public string Alvorlighetsgrad { get; set; }

            public string Beskrivelse { get; set; }

            public Feiltype SkyldigFeiltype { get; set; }
        }
    }
}