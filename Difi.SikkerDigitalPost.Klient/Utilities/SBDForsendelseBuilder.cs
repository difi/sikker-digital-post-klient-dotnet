using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class SBDForsendelseBuilder
    {
        public static StandardBusinessDocument BuildSBD(Forsendelse forsendelse)
        {
            ForretningsMelding forretningsMelding = ForretningsMeldingBuilder.BuildForretningsMelding(forsendelse);

            var konversasjonsId = forsendelse.KonversasjonsId.ToString();

            StandardBusinessDocumentHeader sbdHeader = new StandardBusinessDocumentHeader.Builder()
                .WithProcess(forsendelse.PostInfo is DigitalPostInfo ? Process.ProcessType.DIGITAL_POST_INFO : Process.ProcessType.DIGITAL_POST_VEDTAK)
                .WithStandard(forsendelse.PostInfo.Type)
                .WithFrom(forsendelse.Avsender.Organisasjonsnummer) //TODO: From databehandler.
                .WithOnBehalfOf(forsendelse.Avsender.Organisasjonsnummer)
                .WithTo(forsendelse.MottakerPersonIdentifikator)
                .WithType(forretningsMelding.type.ToString())
                .WithRelatedToConversationId(konversasjonsId)
                .WithRelatedToMessageId(konversasjonsId)
                .WithCreationDateAndTime(DateTime.Now.ToLocalTime())
                .Build();
            
            StandardBusinessDocument sbd = new StandardBusinessDocument();
            sbd.standardBusinessDocumentHeader = sbdHeader;
            sbd.any = forretningsMelding;

            return sbd;
        }
    }
}
