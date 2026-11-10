using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveClub.Firma.Services
{
    public class AuthMessageHandler : DelegatingHandler
    {
        private readonly UserSession _session;

        public AuthMessageHandler(UserSession session)
        {
            _session = session;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_session.Token))
            {
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", _session.Token);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}