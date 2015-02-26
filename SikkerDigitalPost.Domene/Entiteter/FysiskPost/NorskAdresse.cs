using System;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class NorskAdresse : Adresse
    {
        public string Postnummer { get; set; }

        public string Poststed { get; set; }

        public NorskAdresse(string postnummer, string poststed)
        {
            Postnummer = postnummer;
            Poststed = poststed;
        }
    }
}