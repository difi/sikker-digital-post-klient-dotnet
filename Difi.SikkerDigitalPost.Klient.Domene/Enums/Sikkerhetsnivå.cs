namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    public enum Sikkerhetsnivå
    {
        /// <summary>
        /// "Mellomhøyt" sikkerhetsnivå.
        /// 
        /// Vanligvis passord.
        /// </summary>
        Nivå3 = 3,

        /// <summary>
        /// Offentlig godkjent to-faktor elektronisk ID.
        /// 
        /// For eksempel BankID, Buypass eller Comfides.
        /// </summary>
        Nivå4 = 4
    }
}
