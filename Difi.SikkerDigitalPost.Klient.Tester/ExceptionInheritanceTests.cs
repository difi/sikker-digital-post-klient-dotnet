using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class ExceptionInheritanceTests
    {
        [TestMethod]
        public void DoesItwork()
        {
            var exception = new InheritedClass();
        }

        public class InheritedClass : DifiException
        {

        }

        [Serializable]
        public class DifiException : Exception
        {
            public DifiException()
            {
            }

            public DifiException(string message)
              : base(message)
            {
            }

            public DifiException(string message, Exception inner)
              : base(message, inner)
            {
            }
        }
    }


}
