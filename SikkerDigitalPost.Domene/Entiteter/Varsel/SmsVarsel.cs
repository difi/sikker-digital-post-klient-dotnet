using System.Collections.Generic;
using System.Linq;

namespace SikkerDigitalPost.Domene.Entiteter.Varsel
{

    /// <summary>
    /// Informasjon om hvordan postkasseleverandør skal varsle Mottaker om den nye posten. 
    /// 
    /// Varslingsinformasjonen angitt her vil overstyre Mottaker sine egne varslingspreferanser; det vil kunne 
    /// komme som tillegg til Mottaker sine varslingvalg. Avsender kan med instillingene her styre både 
    /// EpostVarsel og SmsVarsel helt uavhengig av hverandre. Det vil si at Avsender kan velge å varsle i begge
    /// eller en av kanalene. Avsender kan velge selv hvilken kanal som velges, dette kan de gjøre med bakgrunn
    /// i sin egen kanalstrategi, erfaringer i forhold til åpningsgrad og kostnader. Bruk av SmsVarsel vil
    /// medføre egne kostnader for Avsender. Se http://begrep.difi.no/SikkerDigitalPost/1.0.1/begrep/Varsler
    /// for mer informasjon.
    /// </summary>
    public class SmsVarsel : Varsel
    {
        /// <summary>
        /// Mobiltelefonnummeret varselet skal sendes til. For informasjon om validering, se
        /// http://begrep.difi.no/Felles/mobiltelefonnummer. 
        /// </summary>
        /// <remarks></remarks>
        public readonly string Mobilnummer;

        /// <param name="mobilnummer">Mobiltelefonnummer varselet skal sendes til.</param>
        /// <param name="varslingstekst">Avsenderstyrt varslingstekst som skal inngå i varselet.</param>
        public SmsVarsel(string mobilnummer,string varslingstekst, IEnumerable<int> varselEtterDager) : base(varslingstekst,varselEtterDager)
        {
            Mobilnummer = mobilnummer;
        }

        public SmsVarsel(string mobilnummer, string varslingstekst, params int[] varselEtterDager)
            : this(mobilnummer, varslingstekst, varselEtterDager.ToList())
        {
            
        }

    }
}
