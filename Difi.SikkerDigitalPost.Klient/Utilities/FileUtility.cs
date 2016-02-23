using System;
using System.IO;
using System.Xml.Linq;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class FileUtility
    {
        private static string _basePath;

        public static string BasePath
        {
            get { return _basePath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); }
            set { _basePath = value; }
        }

        /// <summary>
        ///     Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        ///     på "C:\base\mappe\hei.txt".
        /// </summary>
        /// <param name="xml">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void WriteXmlToBasePath(string xml, params string[] pathRelativeToBase)
        {
            if (string.IsNullOrEmpty(xml))
                return;

            var doc = XDocument.Parse(xml);
            WriteToBasePath(doc.ToString(), pathRelativeToBase);
        }

        /// <summary>
        ///     Hvis BasePath er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        ///     på "C:\base\mappe\hei.txt".
        /// </summary>
        /// <param name="data">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void WriteToBasePath(string data, params string[] pathRelativeToBase)
        {
            var absolutePath = AbsolutePath(pathRelativeToBase);
            Write(data, absolutePath);
        }

        public static void WriteToBasePath(byte[] data, params string[] pathRelativeToBase)
        {
            var absolutePath = AbsolutePath(pathRelativeToBase);
            Write(data, absolutePath);
        }

        public static void Write(string data, string absolutePath)
        {
            if (string.IsNullOrEmpty(data))
                return;

            CreateDirectory(absolutePath);
            File.WriteAllText(absolutePath, data);
        }

        public static void Write(byte[] data, string absolutePath)
        {
            if (data.Length == 0)
                return;

            CreateDirectory(absolutePath);
            File.WriteAllBytes(absolutePath, data);
        }

        /// <summary>
        ///     Hvis din basesti er "C:\base" og du sender inn "mappe\hei.txt", så vil filen lagres
        ///     på "C:\base\mappe\hei.txt". Legg er tekst til allerede eksisterende tekst.
        /// </summary>
        /// <param name="data">Data som skal skrives.</param>
        /// <param name="pathRelativeToBase">Relativ del av stien. Den absolutte delen er i FileUtility.BasePath </param>
        public static void AppendToFileInBasePath(string data, params string[] pathRelativeToBase)
        {
            if (string.IsNullOrEmpty(data))
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