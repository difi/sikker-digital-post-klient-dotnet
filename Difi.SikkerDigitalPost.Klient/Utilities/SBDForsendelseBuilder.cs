using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class SBDForsendelseBuilder
    {
        public static StandardBusinessDocument BuildSBD(Forsendelse forsendelse)
        {
            ForretningsMelding forretningsMelding = ForretningsMeldingBuilder.BuildForretningsMelding(forsendelse);

            StandardBusinessDocument sbd = new StandardBusinessDocument();

            string konversasjonsId = forsendelse.KonversasjonsId.ToString();
            
            StandardBusinessDocumentHeader sbdHeader = new StandardBusinessDocumentHeader.Builder().WithProcess(Process.ProcessType.DIGITAL_POST_INFO)
                .WithStandard(forsendelse.PostInfo.Type)
                //.WithFrom(databehandler).onBehalfOf(avsender.getOrganisasjonsnummer())
                .WithTo(forsendelse.PostInfo.Mottaker as DigitalPostMottaker)
                .WithType(forretningsMelding.type.ToString())
                .WithRelatedToConversationId(konversasjonsId)
                .WithRelatedToMessageId(konversasjonsId)
                .WithCreationDateAndTime(DateTime.Now.ToLocalTime())
                .Build();

            sbd.standardBusinessDocumentHeader = sbdHeader;
            sbd.any = forretningsMelding;

            return sbd;
        }
    }
}
