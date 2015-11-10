using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    /// <summary>
    /// Abstrakt klasse for transportkvitteringer.
    /// </summary>
    public abstract class Transportkvittering : Kvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public abstract override string ToString();
        
        protected Transportkvittering()
        { }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
            : base(document, namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                   String.Format("Feil under bygging av {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                   , GetType()), e);
            }
        }
    }
}
