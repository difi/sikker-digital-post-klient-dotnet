namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    public abstract class Transportkvittering : Kvittering
    {
        protected Transportkvittering()
            : base(string.Empty)
        {
        }

        public long AntallBytesDokumentpakke { get; set; }
    }
}