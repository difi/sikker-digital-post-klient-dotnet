namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    public enum Feiltype
    {
        /// <summary>
        ///     Feil som har oppstått som følge av en feil hos klienten.
        /// </summary>
        Klient,

        /// <summary>
        ///     Feil som har oppstått som følge av feil på sentral infrastruktur. Bør meldes til sentralforvalter.
        /// </summary>
        Server
    }
}