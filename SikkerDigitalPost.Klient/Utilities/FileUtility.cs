/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.IO;
using System.Xml.Linq;

namespace SikkerDigitalPost.Klient.Utilities
{
    internal class FileUtility
    {
        enum Bruker
        {
            Aleksander,
        }

        private static Bruker CurrentBruker
        {
            get
            {
                if (Environment.MachineName.Contains("LEK"))
                {
                    return Bruker.Aleksander;
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
                    return @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data";
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
            if (String.IsNullOrEmpty(xml))
                return;

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
            if (String.IsNullOrEmpty(data))
                return;

            var absolutePath = AbsolutePath(pathRelativeToBase);
            CreateDirectory(absolutePath);
            File.WriteAllText(absolutePath, data);
        }

        public static void WriteToFileInBasePath(byte[] data, params string[] pathRelativeToBase)
        {
            if (data.Length == 0)
                return;

            var absolutePath = AbsolutePath(pathRelativeToBase);
            CreateDirectory(absolutePath);
            File.WriteAllBytes(absolutePath, data);
        }

        /// <summary>
        /// Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        /// på "C:\base\mappe\hei.txt". Legg er tekst til allerede eksisterende tekst.
        /// </summary>
        /// <param name="data">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void AppendToFileInBasePath(string data, params string[] pathRelativeToBase)
        {
            if (String.IsNullOrEmpty(data))
                return;

            var absolutePath = AbsolutePath(pathRelativeToBase);
            CreateDirectory(absolutePath);
            File.AppendAllText(absolutePath, data);
        }

        public static string AbsolutePath(params string[] pathRelativeToBase)
        {
            return Path.Combine(BasePath, Path.Combine(pathRelativeToBase));
        }

        private static void CreateDirectory(string path)
        {
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
        }
    }
}

