using System;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class GuidUtility
    {
        public string MessageId { get; } = Guid.NewGuid().ToString();
    }
}
