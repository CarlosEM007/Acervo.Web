using System.Net.Http.Headers;

namespace Acervo.Web.Service
{
    /// <summary>
    /// Anexa o token JWT armazenado na <see cref="SessionService"/> ao cabeçalho
    /// Authorization de toda requisição enviada à API.
    /// </summary>
    public class AuthTokenHandler(SessionService session) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(session.Token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
