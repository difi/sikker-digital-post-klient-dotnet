using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class SBDForsendelseBuilder
    {
        public static StandardBusinessDocument BuildSBD(Forsendelse forsendelse)
        {
            ForretningsMelding forretningsMelding = new ForretningsMelding();
            forretningsMelding.type = ForretningsMeldingType.DIGITAL;
            forretningsMelding.hoveddokument = forsendelse.Dokumentpakke.Hoveddokument.Filnavn;
            
            //Avsender avsender = forsendelse.getAvsender();
            //forretningsMelding.setAvsenderId(avsender.getAvsenderIdentifikator());
            //forretningsMelding.setFakturaReferanse(avsender.getFakturaReferanse());

//            if(forsendelse.type == DIGITAL) {
//                forsendelse.getDokumentpakke().getHoveddokumentOgVedlegg()
//                    .filter(dokument -> dokument.getMetadataDocument().isPresent())
//                    .forEach(dokument -> ((DigitalPost)forretningsMelding).addMetadataMapping(dokument.getFileName(), dokument.getMetadataDocument().get().getFileName()));
//            }

            StandardBusinessDocument sbd = new StandardBusinessDocument();

            string konversasjonsId = forsendelse.KonversasjonsId.ToString();
            
            StandardBusinessDocumentHeader sbdHeader = new StandardBusinessDocumentHeader.Builder().WithProcess(Process.ProcessType.DIGITAL_POST_INFO)
                .WithStandard(forsendelse.PostInfo.Type)
                //.WithFrom(databehandler).onBehalfOf(avsender.getOrganisasjonsnummer())
                .WithTo(forsendelse.PostInfo.Mottaker as DigitalPostMottaker)
                .WithType(forretningsMelding.type.ToString())
                .WithRelatedToConversationId(konversasjonsId)
                .WithRelatedToMessageId(konversasjonsId)
                .WithCreationDateAndTime(DateTime.Now)
                .Build();

            sbd.standardBusinessDocumentHeader = sbdHeader;
            sbd.any = forretningsMelding;

            return sbd;
        }
    }
}
