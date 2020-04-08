using System.ComponentModel;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class Process
    {
        public enum ProcessType
        {
            DIGITAL_POST_INFO,
            DIGITAL_POST_VEDTAK,
        }

        public static string GetEnumDescription(ProcessType processType)
        {
            switch (processType)
            {
                case ProcessType.DIGITAL_POST_INFO:
                    return "urn:no:difi:profile:digitalpost:info:ver1.0";
                case ProcessType.DIGITAL_POST_VEDTAK:
                    return "urn:no:difi:profile:digitalpost:vedtak:ver1.0";
                default:
                    return "";
            }
        }
    }
}
