using System.Text;
using System.Text.Json;

namespace Acervo.Web.Service
{
    public class SessionService
    {
        public string? Token { get; private set; }
        public long?   UserId { get; private set; }
        public string? Email { get; private set; }

        public bool IsAuthenticated => Token is not null;

        public void SetToken(string token)
        {
            Token = token;
            ParseClaims(token);
        }

        public void Clear()
        {
            Token  = null;
            UserId = null;
            Email  = null;
        }

        // Decodifica o payload do JWT (sem validar assinatura — isso é papel da API)
        // apenas para saber quem é o usuário logado no front.
        private void ParseClaims(string token)
        {
            UserId = null;
            Email  = null;

            try
            {
                var parts = token.Split('.');
                if (parts.Length < 2) return;

                using var doc = JsonDocument.Parse(Base64UrlDecode(parts[1]));
                var root = doc.RootElement;

                if (root.TryGetProperty("sub", out var sub) &&
                    long.TryParse(sub.GetString(), out var id))
                    UserId = id;

                if (root.TryGetProperty("email", out var email))
                    Email = email.GetString();
            }
            catch
            {
                // Token malformado — mantém sessão sem claims decodificadas.
            }
        }

        private static string Base64UrlDecode(string input)
        {
            var s = input.Replace('-', '+').Replace('_', '/');
            s += (s.Length % 4) switch { 2 => "==", 3 => "=", _ => "" };
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
    }
}
