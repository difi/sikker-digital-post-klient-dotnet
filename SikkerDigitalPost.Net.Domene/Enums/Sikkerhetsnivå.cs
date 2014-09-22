namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public enum Sikkerhetsnivå
    {
        /// <summary>
        /// "Mellomhøyt" sikkerhetsnivå.
        /// 
        /// Vanligvis passord.
        /// </summary>
        Nivå3,

        /// <summary>
        /// Offentlig godkjent to-faktor elektronisk ID.
        /// 
        /// For eksempel BankID, Buypass eller Comfides.
        /// </summary>
        Nivå4
    }
}
