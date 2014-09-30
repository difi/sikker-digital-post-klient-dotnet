using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class DigitalPostElement : XmlPart
    {
        private const string Ns9 = "http://begrep.difi.no/sdp/schema_v10";

        public DigitalPostElement(XmlDocument dokument, Forsendelse forsendelse) : base(dokument, forsendelse)
        {
        }

        public override XmlElement Xml()
        {
            XmlElement digitalPostElement = XmlDocument.CreateElement("ns9", "digitalPost", Ns9);
            {
                //digitalPostElement.AppendChild(SignatureElement());
                digitalPostElement.AppendChild(AvsenderElement());
                digitalPostElement.AppendChild(MottakerElement());
                digitalPostElement.AppendChild(DigitalPostInfoElement());
                //digitalPostElement.AppendChild(DokumentfingerpakkeavtrykkElement());
            }
            return digitalPostElement;
        }

        private XmlElement SignatureElement()
        {
            return null;
        }

        private XmlElement AvsenderElement()
        {
            XmlElement avsender = XmlDocument.CreateElement("ns9", "avsender", Ns9);
            {
                XmlElement organisasjon = XmlDocument.CreateElement("ns9", "organisasjon", Ns9);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
                avsender.AppendChild(organisasjon);
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = XmlDocument.CreateElement("ns9", "mottaker", Ns9);
            {
                XmlElement person = XmlDocument.CreateElement("ns9", "person", Ns9);
                {
                    XmlElement personidentifikator = XmlDocument.CreateElement("ns9", "personidentifikator", Ns9);
                    personidentifikator.InnerText = Forsendelse.DigitalPost.Mottaker.Personidentifikator;
                    person.AppendChild(personidentifikator);

                    XmlElement postkasseadresse = XmlDocument.CreateElement("ns9", "postkasseadresse", Ns9);
                    postkasseadresse.InnerText = Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                    person.AppendChild(postkasseadresse);
                }
                mottaker.AppendChild(person);
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = XmlDocument.CreateElement("ns9", "digitalPostInfo", Ns9);
            {
                XmlElement aapningskvittering = XmlDocument.CreateElement("ns9", "aapningskvittering", Ns9);
                aapningskvittering.InnerText = Forsendelse.DigitalPost.Åpningskvittering.ToString();
                digitalPostInfo.AppendChild(aapningskvittering);

                XmlElement sikkerhetsnivaa = XmlDocument.CreateElement("ns9", "sikkerhetsnivaa", Ns9);
                sikkerhetsnivaa.InnerText = Forsendelse.DigitalPost.Sikkerhetsnivå.ToString();
                digitalPostInfo.AppendChild(sikkerhetsnivaa);

                XmlElement ikkeSensitivTittel = XmlDocument.CreateElement("ns9", "ikkeSensitivTittel", Ns9);
                ikkeSensitivTittel.InnerText = Forsendelse.DigitalPost.IkkeSensitivTittel;
                digitalPostInfo.AppendChild(ikkeSensitivTittel);

                XmlElement varsler = XmlDocument.CreateElement("ns9", "varsler", Ns9);
                //
                //
            }
            return digitalPostInfo;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            return null;
        }
    }
}
