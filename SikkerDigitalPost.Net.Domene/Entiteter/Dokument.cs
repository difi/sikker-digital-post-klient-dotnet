using System;
using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter;

namespace SikkerDigitalPost.Net.Domene
{
    public class Dokument : IAsiceAttachable
    {
        private readonly string _dokumentsti;

        public string Filnavn { get; private set; }
        public byte[] Bytes { get; private set; }
        public string MimeType { get; private set; }
        public string Tittel { get; private set; }
        
        public Dokument(string tittel, string dokumentsti)
        {
            Tittel = tittel;
            _dokumentsti = dokumentsti;

            try
            {
                this.Bytes = File.ReadAllBytes(dokumentsti);
            }
            catch (Exception e)
            {
                throw new Exception("Kunne ikke lese fra fil", e);
            }
        }
    }
}
