namespace Acervo.Web.DTOs
{
    public record LibraryItemDto(
        long      Id,
        long      LibraryId,
        long      BookId,
        DateTime  AcquiredAt,
        DateTime? LastReadAt,
        int       ReadingProgress);

    public record CreateLibraryItemDto(long LibraryId, long BookId);

    public record UpdateLibraryItemDto(long Id, long LibraryId, long BookId);
}
