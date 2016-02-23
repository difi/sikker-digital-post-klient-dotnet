using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    public class Dokument : IAsiceVedlegg
    {
        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentsti">Stien som viser til hvor dokumentet ligger på disk.</param>
        /// <param name="mimeType">
        ///     MimeType for dokumentet. For informasjon om tillatte formater, se
        ///     http://begrep.difi.no/SikkerDigitalPost/1.0.3/forretningslag/Dokumentformat/.
        /// </param>
        /// <param name="språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse" /> sitt språk.</param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        public Dokument(string tittel, string dokumentsti, string mimeType, string språkkode = null, string filnavn = null)
            : this(tittel, File.ReadAllBytes(dokumentsti), mimeType, språkkode, filnavn ?? Path.GetFileName(dokumentsti))
        {
        }

        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentstrøm">Dokumentet representert som en strøm.</param>
        /// <param name="mimeType">
        ///     MimeType for dokumentet. For informasjon om tillatte formater, se
        ///     http://begrep.difi.no/SikkerDigitalPost/1.0.3/forretningslag/Dokumentformat/.
        /// </param>
        /// <param name="språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse" /> sitt språk.</param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        public Dokument(string tittel, Stream dokumentstrøm, string mimeType, string språkkode = null, string filnavn = null)
            : this(tittel, File.ReadAllBytes(new StreamReader(dokumentstrøm).ReadToEnd()), mimeType, språkkode, filnavn)
        {
        }

        /// <param name="tittel">Tittel som vises til brukeren gitt riktig sikkerhetsnivå.</param>
        /// <param name="dokumentbytes">Dokumentet representert som byte[].</param>
        /// <param name="mimeType">
        ///     MimeType for dokumentet. For informasjon om tillatte formater, se
        ///     http://begrep.difi.no/SikkerDigitalPost/1.0.3/forretningslag/Dokumentformat/.
        /// </param>
        /// <param name="språkkode">Språkkode for dokumentet. Om ikke satt, brukes <see cref="Forsendelse" /> sitt språk.</param>
        /// <param name="filnavn">Filnavnet til dokumentet.</param>
        public Dokument(string tittel, byte[] dokumentbytes, string mimeType, string språkkode = null, string filnavn = null)
        {
            filnavn = filnavn ?? Path.GetRandomFileName();
            var vasketFilnavn = filnavn.RemoveIllegalCharacters();

            Tittel = tittel;
            Bytes = dokumentbytes;
            MimeType = mimeType;
            Filnavn = UrlEncode(filnavn.RemoveIllegalCharacters());
            FilnavnRådata = vasketFilnavn;
            Språkkode = språkkode;
        }

        public string Tittel { get; private set; }

        public string Språkkode { get; set; }

        internal string FilnavnRådata { get; set; }

        public string Filnavn { get; }

        public byte[] Bytes { get; }

        public string MimeType { get; }

        public string Id { get; set; }

        private string UrlEncode(string raw)
        {
            var result = HttpUtility.UrlEncode(raw, new UTF8Encoding());
            result = Regex.Replace(result, "%[a-z0-9]{2}", m => m.Value.ToUpperInvariant());
            return result;
        }
    }
}