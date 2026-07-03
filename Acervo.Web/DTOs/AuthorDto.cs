namespace Acervo.Web.DTOs
{
    public record AuthorDto(
        long      Id,
        string    Name,
        string?   Biography,
        DateTime? BirthDate);

    public record CreateAuthorDto(string Name, string? Biography, DateTime? BirthDate);

    public record UpdateAuthorDto(long Id, string Name, string? Biography, DateTime? BirthDate);
}
