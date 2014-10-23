using System;
using System.Xml;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// En kvittering som Behandlingsansvarlig kan oppbevare som garanti på at posten er levert til Mottaker. 
    /// Kvitteringen sendes fra Postkassleverandør når postforsendelsen er validert og de garanterer for at posten vil bli tilgjengeliggjort.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/LeveringsKvittering.
    /// </summary>
    public class Leveringskvittering : Forretningskvittering
    {
        internal Leveringskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager) : base(xmlDocument,namespaceManager)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nKonversasjonsId: {3}. \nRefererer til melding med id: {4}", 
                GetType().Name, MeldingsId, Tidspunkt, KonversasjonsId, RefToMessageId);
        }
    }
}
