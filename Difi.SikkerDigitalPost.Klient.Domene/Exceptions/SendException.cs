using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Exceptions
{
    [SerializableAttribute]
    public class SendException : SikkerDigitalPostException
    {
        public SendException()
        {

        }

        public SendException(string message)
            : base(message)
        {

        }

        public SendException(string message, Exception inner)
            : base(message, inner)
        {

        }


    }
}
