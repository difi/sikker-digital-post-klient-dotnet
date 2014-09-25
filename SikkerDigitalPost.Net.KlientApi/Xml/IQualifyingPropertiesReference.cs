namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    public interface IQualifyingPropertiesReference
    {
        string Filename { get; set; }

        string Mimetype { get; set; }
    }

    internal class QualifyingPropertiesReference : IQualifyingPropertiesReference
    {
        public string Filename { get; set; }

        public string Mimetype { get; set; }
    }
}
