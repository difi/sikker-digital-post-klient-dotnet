namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    public enum Sikkerhetsnivå
    {
        /// <summary>
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>MinID (nivå 3)</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 Innlogging med fødselsnummer/passord, deretter med kode fra enten SMS eller PIN-kode brev.
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 Dette er den mest brukte innloggingsmekanismen i ID-porten
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        Nivå3 = 3,

        /// <summary>
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>BankID (nivå 4)</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 Innlogging med kodebrikke fra banken din
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 ID-porten støtter kun BankID på mobil fra bankene DnB, Sparebank 1 og Eika
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 ID-porten støtter BankID 2.0 (uten behov for Java)
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        Nivå4 = 4
    }
}