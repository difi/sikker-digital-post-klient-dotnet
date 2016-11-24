using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
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

            return new Leveringskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Mottakskvittering TilMottakskvittering(XmlDocument mottakskvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(mottakskvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(mottakskvitteringXmlDocument);

            return new Mottakskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Returpostkvittering TilReturpostkvittering(XmlDocument returpostkvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(returpostkvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(returpostkvitteringXmlDocument);

            return new Returpostkvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static VarslingFeiletKvittering TilVarslingFeiletKvittering(XmlDocument varslingFeiletXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(varslingFeiletXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(varslingFeiletXmlDocument);
            var varslingfeiletKvitteringsfelter = HentVarslingFeiletKvitteringsfelter(varslingFeiletXmlDocument);

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

        public static Åpningskvittering TilÅpningskvittering(XmlDocument åpningskvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(åpningskvitteringXmlDocument);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(åpningskvitteringXmlDocument);

            return new Åpningskvittering(kvitteringFelter.MeldingsId, forretningskvitteringfelter.KonversasjonsId, forretningskvitteringfelter.BodyReferenceUri, forretningskvitteringfelter.DigestValue)
            {
                Generert = forretningskvitteringfelter.Generert,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                Xml = kvitteringFelter.Xml,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt
            };
        }

        public static Feilmelding TilFeilmelding(XmlDocument feilmelding)
        {
            var kvitteringFelter = HentKvitteringsfelter(feilmelding);
            var forretningskvitteringfelter = HentForretningskvitteringFelter(feilmelding);
            var feilmeldingfelter = HentFeilmeldingsfelter(feilmelding);

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

        public static TomKøKvittering TilTomKøKvittering(XmlDocument tomKøkvitteringXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(tomKøkvitteringXmlDocument);

            return new TomKøKvittering
            {
                MeldingsId = kvitteringFelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringFelter.ReferanseTilMeldingId,
                SendtTidspunkt = kvitteringFelter.SendtTidspunkt,
                Xml = kvitteringFelter.Xml
            };
        }

        public static TransportFeiletKvittering TilTransportFeiletKvittering(XmlDocument transportFeiletXmlDocument)
        {
            var kvitteringFelter = HentKvitteringsfelter(transportFeiletXmlDocument, false);
            var transportFeiletFelter = HentTransportFeiletKvitteringsfelter(transportFeiletXmlDocument);

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

        private static Kvitteringsfelter HentKvitteringsfelter(XmlDocument kvittering, bool sjekkEtterReferanseTilMeldingsId = true)
        {
            try
            {
                return ParseKvitteringsFelter(kvittering, sjekkEtterReferanseTilMeldingsId);
            }
            catch (Exception e)
            {
                throw new XmlParseException($"Feil under bygging av {e.GetType()} (av type Kvittering).", e);
            }
        }

        private static Kvitteringsfelter ParseKvitteringsFelter(XmlDocument kvittering, bool sjekkEtterReferanseTilMeldingsId)
        {
            return new Kvitteringsfelter
            {
                SendtTidspunkt = Convert.ToDateTime(GetXmlNodeFromDocument(kvittering, "//ns6:Timestamp").InnerText),
                MeldingsId = GetXmlNodeFromDocument(kvittering, "//ns6:MessageId").InnerText,
                Xml = kvittering,
                ReferanseTilMeldingId = sjekkEtterReferanseTilMeldingsId ? GetXmlNodeFromDocument(kvittering, "//ns6:RefToMessageId").InnerText : null
            };
        }

        public static TransportOkKvittering TilTransportOkKvittering(XmlDocument transportOkXmlDocument)
        {
            var kvitteringsfelter = HentKvitteringsfelter(transportOkXmlDocument);

            return new TransportOkKvittering
            {
                MeldingsId = kvitteringsfelter.MeldingsId,
                ReferanseTilMeldingId = kvitteringsfelter.ReferanseTilMeldingId,
                SendtTidspunkt = kvitteringsfelter.SendtTidspunkt,
                Xml = kvitteringsfelter.Xml
            };
        }

        private static Forretningskvitteringfelter HentForretningskvitteringFelter(XmlDocument forretningskvittering)
        {
            var forretningskvittergFelter = new Forretningskvitteringfelter();

            var bodyId = SjekkForretningskvitteringForKonsistens(forretningskvittering);

            try
            {
                var guidNode = GetXmlNodeFromDocument(forretningskvittering, "//ns3:BusinessScope/ns3:Scope/ns3:InstanceIdentifier");
                forretningskvittergFelter.KonversasjonsId = new Guid(guidNode.InnerText);

                var tidspunktNode = GetXmlNodeFromDocument(forretningskvittering, "//ns9:tidspunkt");
                forretningskvittergFelter.Generert = Convert.ToDateTime(tidspunktNode.InnerText);

                var bodyReferenceNode = forretningskvittering.SelectSingleNode("//ns5:Reference[@URI = '#" + bodyId + "']", GetNamespaceManager(forretningskvittering));
                forretningskvittergFelter.BodyReferenceUri = bodyReferenceNode.Attributes["URI"].Value;
                forretningskvittergFelter.DigestValue = bodyReferenceNode.SelectSingleNode("//ds:DigestValue", GetNamespaceManager(forretningskvittering)).InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException($"Feil under bygging av {e.GetType()} (av type Forretningskvittering).", e);
            }

            return forretningskvittergFelter;
        }

        private static string SjekkForretningskvitteringForKonsistens(XmlDocument document)
        {
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
                    "Feil under bygging av VarslingFeilet-kvittering.", e);
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
                throw new XmlParseException("Feil under bygging av Feilmelding-kvittering.", e);
            }

            return feilmeldingsfelter;
        }

        private static TransportFeiletKvitteringsfelter HentTransportFeiletKvitteringsfelter(XmlDocument document)
        {
            var transportFeiletKvitteringsfelter = new TransportFeiletKvitteringsfelter();

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