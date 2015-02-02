using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SikkerDigitalPost.Tester
{
    public class ResourceUtility
    {
        private static string _basePath;
        public static string BasePath
        {
            get { return _basePath ?? "SikkerDigitalPost.Tester.testdata"; }
            set { _basePath = value; }
        }

        public static IEnumerable<string> GetFiles(params string[] pathRelativeToBase)
        {
            var path = GetFullPath(pathRelativeToBase);

            return GetAllFiles().Where(file => file.Contains(path));
        }

        private static IEnumerable<string> GetAllFiles()
        {
            return GetAssembly().GetManifestResourceNames();
        }

        private static Assembly GetAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        private static string GetFullPath(params string[] path)
        {
            return String.Join(".", BasePath, String.Join(".", path));
        }

        public static byte[] FileToBytes(bool isRelative, params string[] path)
        {
            var fullpath = isRelative ? GetFullPath(path) : String.Join(".", path);

            var assembly = GetAssembly();

            using (Stream fileStream = assembly.GetManifestResourceStream(fullpath))
            {
                if (fileStream == null) return null;
                byte[] bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }

        }

    }
}
