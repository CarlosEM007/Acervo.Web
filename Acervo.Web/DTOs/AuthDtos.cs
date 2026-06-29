namespace Acervo.Web.DTOs
{
    public class LoginDto
    {
        public string Email        { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }

    public class TokenDto
    {
        public string   Token     { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
    }
}
