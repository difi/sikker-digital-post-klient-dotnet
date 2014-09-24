using System;
using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokument : IAsiceVedlegg
    {
        public string Tittel { get; private set; }
        public string Filnavn { get; set; }
        public byte[] Bytes { get; private set; }
        public string Innholdstype { get; private set; }
        public string Språkkode { get; private set; }
        public bool HarStandardSpråk { get; private set; }

        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentsti">Stien som viser til hvor dokumentet ligger på disk.</param>
        /// <param name="innholdstype">Innholdstype for dokumentet. For informasjon om tillatte formater, se http://begrep.difi.no/SikkerDigitalPost/Dokumentformat/. </param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        /// <param name="Språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse"/> sitt språk.</param>
        public Dokument(string tittel, string dokumentsti, string innholdstype, string filnavn = null, string språkkode = "Forsendelsesspråkkode")
            : this(tittel, File.ReadAllBytes(dokumentsti), innholdstype, filnavn ?? Path.GetFileName(dokumentsti), språkkode)
        {
        }

        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentstrøm">Dokumentet representert som en strøm.</param>
        /// <param name="innholdstype">Innholdstype for dokumentet. For informasjon om tillatte formater, se http://begrep.difi.no/SikkerDigitalPost/Dokumentformat/. </param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        /// <param name="Språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse"/> sitt språk.</param>
        public Dokument(string tittel, Stream dokumentstrøm, string innholdstype, string filnavn = null, string språkkode = "Forsendelsesspråkkode")
            : this(tittel, File.ReadAllBytes(new StreamReader(dokumentstrøm).ReadToEnd()), innholdstype, filnavn, språkkode)
        {
        }

        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentbytes">Dokumentet representert som byte[].</param>
        /// <param name="innholdstype">Innholdstype for dokumentet. For informasjon om tillatte formater, se http://begrep.difi.no/SikkerDigitalPost/Dokumentformat/. </param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        /// <param name="Språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse"/> sitt språk.</param>
        public Dokument(string tittel, byte[] dokumentbytes, string innholdstype, string filnavn = null, string språkkode = "Forsendelsesspråkkode")
        {
            Tittel = tittel;
            Bytes = dokumentbytes;
            Innholdstype = innholdstype;
            Filnavn = filnavn ?? Path.GetRandomFileName();
            SettSpråkkode(språkkode);
        }

        private void SettSpråkkode(string språkkode)
        {

            if (språkkode.Length > 2)
            {
                Språkkode = "NO";
                HarStandardSpråk = true;
            }
            else
            {
                Språkkode = språkkode;
                HarStandardSpråk = false;
            }
        }
    }
}
