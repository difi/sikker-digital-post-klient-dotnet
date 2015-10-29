using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public interface IComparator
    {
        bool AreEqual(object expected, object actual);

        bool AreEqual(object expected, object actual, out IEnumerable<IDifference> differences);
    }

}
