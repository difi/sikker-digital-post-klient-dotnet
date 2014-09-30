using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class DigitalPost
    {
        private const string Ns9 = "http://begrep.difi.no/sdp/schema_v10";

        private readonly XmlDocument _dokument;
        private readonly Forsendelse _forsendelse;

        public DigitalPost(XmlDocument dokument, Forsendelse forsendelse)
        {
            _forsendelse = forsendelse;
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            XmlElement digitalPostElement = _dokument.CreateElement("ns9", "digitalPost", Ns9);
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
            XmlElement avsender = _dokument.CreateElement("ns9", "avsender", Ns9);
            {
                XmlElement organisasjon = _dokument.CreateElement("ns9", "organisasjon", Ns9);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = _forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
                avsender.AppendChild(organisasjon);
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = _dokument.CreateElement("ns9", "mottaker", Ns9);
            {
                XmlElement person = _dokument.CreateElement("ns9", "person", Ns9);
                {
                    XmlElement personidentifikator = _dokument.CreateElement("ns9", "personidentifikator", Ns9);
                    personidentifikator.InnerText = _forsendelse.DigitalPost.Mottaker.Personidentifikator;
                    person.AppendChild(personidentifikator);

                    XmlElement postkasseadresse = _dokument.CreateElement("ns9", "postkasseadresse", Ns9);
                    postkasseadresse.InnerText = _forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                    person.AppendChild(postkasseadresse);
                }
                mottaker.AppendChild(person);
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = _dokument.CreateElement("ns9", "digitalPostInfo", Ns9);
            {
                XmlElement aapningskvittering = _dokument.CreateElement("ns9", "aapningskvittering", Ns9);
                aapningskvittering.InnerText = _forsendelse.DigitalPost.Åpningskvittering.ToString();
                digitalPostInfo.AppendChild(aapningskvittering);

                XmlElement sikkerhetsnivaa = _dokument.CreateElement("ns9", "sikkerhetsnivaa", Ns9);
                sikkerhetsnivaa.InnerText = _forsendelse.DigitalPost.Sikkerhetsnivå.ToString();
                digitalPostInfo.AppendChild(sikkerhetsnivaa);

                XmlElement ikkeSensitivTittel = _dokument.CreateElement("ns9", "ikkeSensitivTittel", Ns9);
                ikkeSensitivTittel.InnerText = _forsendelse.DigitalPost.IkkeSensitivTittel;
                digitalPostInfo.AppendChild(ikkeSensitivTittel);

                XmlElement varsler = _dokument.CreateElement("ns9", "varsler", Ns9);
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
