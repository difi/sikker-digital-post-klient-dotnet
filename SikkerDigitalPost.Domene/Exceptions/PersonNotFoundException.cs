using System;

namespace SikkerDigitalPost.Net.Domene.Exceptions
{
    public class PersonNotFoundException : NullReferenceException
    {
        public PersonNotFoundException(string message) : base(message)
        {
            
        }
    }
}
