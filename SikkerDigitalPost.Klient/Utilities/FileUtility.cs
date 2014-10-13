using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SikkerDigitalPost.Klient.Utilities
{
    public class FileUtility
    {
        enum Bruker
        {
            Aleksander,
            Marius
        }

        private static Bruker CurrentBruker
        {
            get
            {
                if (Environment.MachineName.Contains("ALEKS"))
                {
                    return Bruker.Aleksander;
                }

                if (Environment.MachineName.Contains("LOK"))
                {
                    return Bruker.Marius;
                }

                throw new ArgumentException("Kunne ikke finne ut hvilken bruker du er! Legg deg selv til i listen.");
            }
        }

        /// <summary>
        /// Returnerer base-sti basert på nåværede bruker.
        /// </summary>
        public static string BasePath
        {
            get
            {
                if (CurrentBruker == Bruker.Aleksander)
                {
                    return @"Z:\Development\Digipost\sdp-data";
                }

                if (CurrentBruker == Bruker.Marius)
                {
                    return @"C:\Prosjekt\DigiPost\Temp\";
                }

                throw new Exception("Kunne ikke finne ut hvilken bruker du er! Legg deg selv til i listen.");
            }
        }

        /// <summary>
        /// Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        /// på "C:\base\mappe\hei.txt".
        /// </summary>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        /// <param name="data">Data som skal skrives.</param>
        public static void WriteXmlToFileInBasePath(string xml, params string[] pathRelativeToBase)
        {
            var doc = XDocument.Parse(xml);
            WriteToFileInBasePath(doc.ToString(), pathRelativeToBase);
        }

        /// <summary>
        /// Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        /// på "C:\base\mappe\hei.txt".
        /// </summary>
        /// <param name="data">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void WriteToFileInBasePath(string data, params string[] pathRelativeToBase)
        {
            File.WriteAllText(AbsolutePath(pathRelativeToBase), data);
        }

        /// <summary>
        /// Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        /// på "C:\base\mappe\hei.txt". Legg er tekst til allerede eksisterende tekst.
        /// </summary>
        /// <param name="data">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void AppendToFileInBasePath(string data, params string[] pathRelativeToBase)
        {
            File.AppendAllText(AbsolutePath(pathRelativeToBase), data);
        }

        public static string AbsolutePath(params string[] pathRelativeToBase)
        {
            return Path.Combine(BasePath, Path.Combine(pathRelativeToBase));
        }
    }
}

