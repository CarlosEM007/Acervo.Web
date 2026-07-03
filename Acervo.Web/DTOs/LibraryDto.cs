namespace Acervo.Web.DTOs
{
    public record LibraryDto(long Id, long UserId, DateTime CreatedAt);

    public record CreateLibraryDto(long UserId);

    public record UpdateLibraryDto(long Id, long UserId);
}
