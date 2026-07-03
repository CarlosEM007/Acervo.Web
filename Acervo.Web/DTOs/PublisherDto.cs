namespace Acervo.Web.DTOs
{
    public record PublisherDto(long Id, string Name, string Country, string Website);

    public record CreatePublisherDto(string Name, string Country, string? Website);

    public record UpdatePublisherDto(long Id, string Name, string Country, string? Website);
}
