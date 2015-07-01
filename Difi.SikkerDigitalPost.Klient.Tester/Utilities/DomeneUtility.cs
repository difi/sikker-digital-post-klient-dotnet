using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public static class DomeneUtility
    {
        static readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata");
        private static readonly string fileExtension;

        public static Dokumentpakke GetSimpleDokumentpakke()
        {
            //Dokumentpakke pakke = new Dokumentpakke();
            return null;
        }

        public static Dokument GetSimpleDokument()
        {
            string hoveddokumentMappe = "hoveddokument";
            var hoveddokument = _resourceUtility.GetFiles(hoveddokumentMappe).ElementAt(0);

            var bytes = _resourceUtility.ReadAllBytes(false, hoveddokument);
            var fileName = _resourceUtility.GetFileName(hoveddokument);
            return new Dokument("Hoveddokument", bytes, "text/xml", "NO", fileName);

        }
        
        public static IEnumerable<Dokument> GetSimpleVedlegg()
        {
            var count = 0;
            var VedleggsMappe = "vedlegg";
            var vedleggsstier = _resourceUtility.GetFiles(VedleggsMappe).ToArray();
           

            return new List<Dokument>(
                    vedleggsstier.Select(
                        v => new Dokument("Vedlegg" + count++,
                            _resourceUtility.ReadAllBytes(false, v), 
                            "text/" + fileExtension, 
                            "NO", _resourceUtility.GetFileName(v))));   
        }

    }
}
