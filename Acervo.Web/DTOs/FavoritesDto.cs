namespace Acervo.Web.DTOs
{
    public record FavoritesDto(long Id, long UserId, DateTime CreatedAt);

    public record CreateFavoritesDto(long UserId);

    public record UpdateFavoritesDto(long Id, long UserId);
}
