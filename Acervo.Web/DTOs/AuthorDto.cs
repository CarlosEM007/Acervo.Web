namespace Acervo.Web.DTOs
{
    public record AuthorDto(
        long      Id,
        string    Name,
        string?   Biography,
        DateTime? BirthDate);
}
