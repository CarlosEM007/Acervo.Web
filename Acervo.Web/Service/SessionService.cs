namespace Acervo.Web.Service
{
    public class SessionService
    {
        public string? Token { get; private set; }

        public void SetToken(string token) => Token = token;
        public void Clear() => Token = null;
        public bool IsAuthenticated => Token is not null;
    }
}
