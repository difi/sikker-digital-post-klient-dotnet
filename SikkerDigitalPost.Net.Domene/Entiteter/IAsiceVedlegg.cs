using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public interface IAsiceAttachable
    {
        string Filnavn { get; }
        byte[] Bytes { get; }
        string MimeType { get; }
    }
}
