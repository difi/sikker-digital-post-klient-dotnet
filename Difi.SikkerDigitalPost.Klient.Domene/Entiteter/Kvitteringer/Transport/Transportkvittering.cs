namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    public abstract class Transportkvittering : Kvittering
    {
        protected Transportkvittering()
            : base(string.Empty)
        {
        }

        /// <summary>
        ///     Angir hvor mange bytes som er i dokumentpakken som ble sendt i henhold til Difis spesifikasjon:
        ///     http://begrep.difi.no/SikkerDigitalPost/1.2.1/forretningslag/meldingsstorrelse og
        ///     http://begrep.difi.no/SikkerDigitalPost/1.2.1/forretningslag/Dokumentpakke/langtidslagring
        /// </summary>
        public long AntallBytesDokumentpakke { get; set; }
    }
}