namespace Acervo.Web.DTOs
{
    public record CartDto(
        long     Id,
        long     UserId,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        decimal  Total);

    public record CreateCartDto(long UserId);

    public record UpdateCartDto(long Id, long UserId);
}
