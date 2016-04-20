using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class AsiceGenerator
    {
        internal static DocumentBundle Create(Forsendelse message, GuidUtility guidUtility, X509Certificate2 certificate, string standardLogPath = "")
        {
            var asiceArchive = new AsiceArchive(message, guidUtility, certificate);
            

            if (!string.IsNullOrEmpty(standardLogPath))
            {
                asiceArchive.LagreTilDisk(standardLogPath, "dokumentpakke", DateUtility.DateForFile() + " - Dokumentpakke.zip");
            }
            
            return new DocumentBundle(asiceArchive.Bytes, asiceArchive.UnzippedContentBytesCount, asiceArchive.ContentId);
        }

    }
}
