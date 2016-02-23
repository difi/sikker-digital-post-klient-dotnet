using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    internal enum PMode
    {
        FormidleDigitalPost,
        FormidleFysiskPost,
        KvitteringsForespoersel
    }

    internal static class PModeHelper
    {
        private const string FormidleDigitalPostReferanse = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleDigitalPostForsendelse";
        private const string FormidleFysiskPostReferanse = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleFysiskPostForsendelse";

        internal static string EnumToRef(PMode pMode)
        {
            switch (pMode)
            {
                case PMode.FormidleDigitalPost:
                    return FormidleDigitalPostReferanse;
                case PMode.FormidleFysiskPost:
                    return FormidleFysiskPostReferanse;
                case PMode.KvitteringsForespoersel:
                    return FormidleDigitalPostReferanse;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pMode), pMode.ToString(), "Fant ingen referanse for angitt pMode");
            }
        }

        internal static PMode FromPostInfo(PostInfo postInfo)
        {
            var type = postInfo.GetType();

            if (type == typeof (FysiskPostInfo))
                return PMode.FormidleFysiskPost;

            if (type == typeof (DigitalPostInfo))
                return PMode.FormidleDigitalPost;

            throw new ArgumentOutOfRangeException(nameof(postInfo), type, "PostInfo har feil type.");
        }
    }
}