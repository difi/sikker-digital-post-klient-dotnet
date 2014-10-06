using System;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class DigitalPost
    {
        /// <param name="mottaker">Mottaker av digital post.</param>
        /// <param name="ikkeSensitivTittel">Ikke-sensitiv tittel på brevet. Denne tittelen vil være synlig under transport av meldingen, og kan vises i mottakerens postkasse.</param>
        /// <param name="sikkerhetsnivå">Nødvendig autentiseringsnivå som kreves av mottaker i postkassen for å åpne brevet. Standardverdi er Nivå4</param>
        /// <param name="åpningskvittering">Ønskes kvittering når brevet blir åpnet av mottaker? Standard er false.</param>
        public DigitalPost(Mottaker mottaker, string ikkeSensitivTittel, Sikkerhetsnivå sikkerhetsnivå = Sikkerhetsnivå.Nivå4, bool åpningskvittering = false)
        {
            Mottaker = mottaker;
            IkkeSensitivTittel = ikkeSensitivTittel;
            Sikkerhetsnivå = sikkerhetsnivå;
            Åpningskvittering = åpningskvittering;
        }

        /// <summary>
        /// Mottaker av digital post.
        /// </summary>
        public Mottaker Mottaker { get; set; }

        /// <summary>
        /// Når brevet tilgjengeliggjøres for mottaker.
        /// 
        /// Standardverdi er nå.
        /// </summary>
        public DateTime Virkningsdato = DateTime.Now;
        
        /// <summary>
        /// Ønskes kvittering når brevet blir åpnet av mottaker?
        /// 
        /// Standard er false.
        /// </summary>
        public bool Åpningskvittering { get; set; }

        /// <summary>
        /// Nødvendig autentiseringsnivå som kreves av mottaker i postkassen for å åpne brevet.
        /// 
        /// Standardverdi er Nivå4
        /// </summary>
        public Sikkerhetsnivå Sikkerhetsnivå { get; set; }
        
        /// <summary>
        /// Ikke-sensitiv tittel på brevet.
        /// Denne tittelen vil være synlig under transport av meldingen, og kan vises i mottakerens postkasse.
        /// </summary>
        public string IkkeSensitivTittel { get; set; }
        
        /// <summary>
        /// Minimum e-postvarsel som skal sendes til mottakker av brevet. Postkassen kan velge å sende andre varsler i tillegg.
        /// 
        /// Standard er standardoppførselen til postkasseleverandøren.
        /// </summary>
        public EpostVarsel EpostVarsel { get; set; }
        
        /// <summary>
        /// Minimum sms-varsel som skal sendes til mottaker av brevet. Postkassen kan velge å sende andre varsler i tillegg.
        /// 
        /// Standard er standardoppførselen til postkasseleverandøren.
        /// </summary>
        public SmsVarsel SmsVarsel { get; set; }
    }
}
