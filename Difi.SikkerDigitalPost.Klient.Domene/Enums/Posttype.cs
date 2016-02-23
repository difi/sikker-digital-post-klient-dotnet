namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    /// <summary>
    ///     Posttype avgjør fremsendingstiden for brevet. Hver posttype har forskjellig pris.
    /// </summary>
    public enum Posttype
    {
        /// <summary>
        ///     Fremsendingstid innen 1-2 virkedager i Norge. Tidligst fremme etter 2-6 virkedager til Europa.
        ///     Tidligst fremme etter 4-8 virkedager til resten av verden.
        /// </summary>
        A,

        /// <summary>
        ///     Framsendingstid innen 3-5 virkedager i Norge. Tidligst fremme etter 4-10 virkedager til Europa.
        ///     Tidligst fremme etter 10-14 virkedager til resten av verden
        /// </summary>
        B
    }
}