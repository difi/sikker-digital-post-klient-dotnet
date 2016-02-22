namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    public abstract class Transportkvittering : Kvittering
    {
        protected Transportkvittering():base(meldingsId: string.Empty)
        {
            
        }
    }
}
