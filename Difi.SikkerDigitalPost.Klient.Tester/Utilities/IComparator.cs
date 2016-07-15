using System.Collections.Generic;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public interface IComparator
    {
        bool Equal(object expected, object actual);

        bool Equal(object expected, object actual, out IEnumerable<IDifference> differences);
    }
}