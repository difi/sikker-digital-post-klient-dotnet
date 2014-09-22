using System;
using SikkerDigitalPost.Net.Domene.Entiteter.Varsel;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class DigitalPost
    {
        /// <summary>
        /// Mottaker av digital post.
        /// </summary>
        public Mottaker Mottaker { get; set; }

        /// <summary>
        /// Når brevet tilgjengeliggjøres for mottaker.
        /// 
        /// Standardverdi er nå.
        /// </summary>
        public DateTime Virkningsdato { get; set; }
        
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
