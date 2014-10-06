using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Domene.Entiteter.Interface
{
    internal interface ISoapVedlegg
    {
        string Filnavn { get; }
        byte[] Bytes { get; }
        string Innholdstype { get; }
        string ContentId { get; }
        string TransferEncoding { get; }
    }
}
