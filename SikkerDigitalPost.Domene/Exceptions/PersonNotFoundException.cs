using System;

namespace SikkerDigitalPost.Domene.Exceptions
{
    public class PersonNotFoundException : NullReferenceException
    {
        public PersonNotFoundException(string message) : base(message)
        {
            
        }
    }
}
