namespace Acervo.Web.DTOs
{
    public record FavoritesItemDto(long Id, long FavoritesId, long BookId, DateTime AddedAt);

    public record CreateFavoritesItemDto(long FavoritesId, long BookId);

    public record UpdateFavoritesItemDto(long Id, long FavoritesId, long BookId);
}
