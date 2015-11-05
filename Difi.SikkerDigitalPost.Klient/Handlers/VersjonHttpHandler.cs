using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Difi.SikkerDigitalPost.Klient.Handlers
{
    internal class VersjonHttpHandler : DelegatingHandler
    {
        public VersjonHttpHandler():base(new HttpClientHandler()) { }
        public VersjonHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            request.Headers.Add("User-Agent", GetAssemblyVersion());

            return await base.SendAsync(request, cancellationToken);
        }

        private static string GetAssemblyVersion()
        {
            var netVersion = Assembly
                    .GetExecutingAssembly()
                    .GetReferencedAssemblies().First(x => x.Name == "System.Core").Version.ToString();

            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return string.Format("difi-sikker-digital-post-klient/{0} (.NET/{1})", assemblyVersion, netVersion);
        }

    }
}
