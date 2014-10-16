using System;

namespace SikkerDigitalPost.Domene.Exceptions
{
    public class KonfigurasjonsException : SikkerDigitalPostException
    {
        public KonfigurasjonsException(){
            
        }

        public KonfigurasjonsException(string message) : base(message)
        {
            
        }

        public KonfigurasjonsException(string message, Exception inner) : base (message, inner)
        {
            
        }


    }
}
