using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Difi.SikkerDigitalPost.Klient.Handlers
{
    internal class UserAgentHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
             HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("User-Agent", GetAssemblyVersion());
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private static string GetAssemblyVersion()
        {
            var netVersion = Assembly
                .GetExecutingAssembly()
                .GetReferencedAssemblies().First(x => x.Name == "System.Core").Version.ToString();

            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

            return $"difi-sikker-digital-post-klient-dotnet/{assemblyVersion} (.NET/{netVersion})";
        }
    }
}