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

    public record CreateBookDto(
        string   Title,
        string   Description,
        DateTime Release,
        int      PagesNumber,
        long     CategoryId,
        long     AuthorId,
        long     PublisherId,
        string?  CoverImageUrl);

    public record UpdateBookDto(
        long     Id,
        string   Title,
        string   Description,
        DateTime Release,
        int      PagesNumber,
        long     CategoryId,
        long     AuthorId,
        long     PublisherId,
        string?  CoverImageUrl);
}
