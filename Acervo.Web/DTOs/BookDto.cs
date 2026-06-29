namespace Acervo.Web.DTOs
{
    public record BookDto(
        long     Id,
        string   Title,
        string   Description,
        int      PagesNumber,
        DateTime Release,
        string   CoverImageUrl,
        long     CategoryId,
        long     AuthorId,
        long     PublisherId);
}
