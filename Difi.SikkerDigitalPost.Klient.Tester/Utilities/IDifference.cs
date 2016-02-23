namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public interface IDifference
    {
        string WhatIsCompared { get; set; }

        object ExpectedValue { get; set; }

        string ActualValue { get; set; }

        string PropertyName { get; set; }
    }
}