using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class SBDForsendelseBuilder
    {
        public static StandardBusinessDocument BuildSBD(Databehandler databehandler, Forsendelse forsendelse)
        {
            ForretningsMelding forretningsMelding = ForretningsMeldingBuilder.BuildForretningsMelding(forsendelse);

            var konversasjonsId = forsendelse.KonversasjonsId.ToString();

            StandardBusinessDocumentHeader sbdHeader = new StandardBusinessDocumentHeader.Builder()
                .WithProcess(Process.ProcessType.DIGITAL_POST_VEDTAK)
                .WithStandard(forsendelse.PostInfo.Type)
                .WithFrom(databehandler.Organisasjonsnummer)
                .WithOnBehalfOf(forsendelse.Avsender.Organisasjonsnummer)
                .WithTo(forsendelse.MottakerPersonIdentifikator)
                .WithType(forretningsMelding.type.ToString())
                .WithRelatedToConversationId(konversasjonsId)
                .WithRelatedToMessageId(konversasjonsId)
                .WithCreationDateAndTime(DateTime.Now.ToLocalTime())
                .Build();

            StandardBusinessDocument sbd = new StandardBusinessDocument
            {
                standardBusinessDocumentHeader = sbdHeader, any = forretningsMelding
            };

            return sbd;
        }
    }
}
