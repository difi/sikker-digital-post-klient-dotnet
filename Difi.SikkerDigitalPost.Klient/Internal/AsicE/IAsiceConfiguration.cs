using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal interface IAsiceConfiguration
    {
        IEnumerable<IDokumentpakkeProsessor> Dokumentpakkeprosessorer { get; set; }
    }
}