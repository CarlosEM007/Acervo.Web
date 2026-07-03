namespace Acervo.Web.DTOs
{
    public record CartItemDto(
        long    Id,
        long    CartId,
        long    BookId,
        decimal UnitPrice,
        int     Quantity,
        decimal SubTotal);

    public record CreateCartItemDto(long CartId, long BookId, decimal UnitPrice, int Quantity);

    public record UpdateCartItemDto(long Id, long CartId, long BookId, decimal UnitPrice, int Quantity);
}
