﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public class ResourceUtility
    {
        /// <summary>
        ///     Use for reading embedded resources from current calling assembly.
        ///     This means that resources witin the assembly creating 
        ///     the resource utility can be found.
        /// </summary>
        /// <param name="basePathForResources">
        ///     Must be in following form
        ///     'SolutionNameSpace.ProjectName.File.Path.Separated.By.Dots>'
        /// </param>
        /// <remarks>The calling assembly will be used as reference. If looking for files inside
        /// the library please use overload taking in assembly as a parameter.</remarks>
        public ResourceUtility(string basePathForResources)
            : this(Assembly.GetExecutingAssembly(), basePathForResources)
        {
        }

        /// <summary>
        ///     Use for reading embedded project resources from an assembly.
        /// </summary>
        /// <param name="currentAssembly">
        ///     The assembly to look for embedded resources in.
        /// </param>
        /// <param name="basePathForResources">
        ///      Must be in following form
        ///     'SolutionNameSpace.ProjectName.File.Path.Separated.By.Dots>'
        /// </param>
        public ResourceUtility(Assembly currentAssembly, string basePathForResources)
        {
            BasePath = IncludeProjectRootNameInBasePath(currentAssembly, basePathForResources);
            CurrentExecutingAssembly = currentAssembly;
        }

        private static string IncludeProjectRootNameInBasePath(Assembly currentAssembly, string basePathForResources)
        {
            var assemblyName = currentAssembly.GetName().Name;

            return !basePathForResources.Contains(assemblyName)
                ? string.Join(".", assemblyName, basePathForResources)
                : basePathForResources;
        }

        public string BasePath { get; }

        public Assembly CurrentExecutingAssembly { get; }

        public IEnumerable<string> GetFiles(params string[] pathRelativeToBase)
        {
            var path = JoinWithBasePath(pathRelativeToBase);

            return GetAllFiles().Where(file => file.Contains(path));
        }

        public string GetFileName(string resource, bool withExtension = true)
        {
            var parts = resource.Split('.');
            var filename = parts[parts.Length - 2];

            if (withExtension)
            {
                var extension = parts[parts.Length - 1];
                filename = string.Format("{0}.{1}", filename, extension);
            }

            return filename;
        }

        public byte[] ReadAllBytes(params string[] path)
        {
            var fullpath = JoinWithBasePath(path);

            using (var fileStream = CurrentExecutingAssembly.GetManifestResourceStream(fullpath))
            {
                if (fileStream == null) return null;
                var bytes = new byte[fileStream.Length];
                fileStream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        private string JoinWithBasePath(params string[] path)
        {
            List<string> elements = new List<string>() {BasePath};
            elements.AddRange(path);

            return string.Join(".", elements);
        }

        private IEnumerable<string> GetAllFiles()
        {
            return CurrentExecutingAssembly.GetManifestResourceNames();
        }
    }
}
