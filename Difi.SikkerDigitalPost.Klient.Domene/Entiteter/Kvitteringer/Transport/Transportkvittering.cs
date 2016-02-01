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
        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
       
        protected Transportkvittering()
        { }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
            : base(document, namespaceManager)
        {
            
        }
    }
}
