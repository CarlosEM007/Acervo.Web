namespace Acervo.Web.DTOs
{
    public record StockItemDto(
        long     Id,
        long     StockId,
        long     BookId,
        int      Quantity,
        decimal  Price,
        DateTime UpdatedAt);

    public record CreateStockItemDto(long StockId, long BookId, int Quantity, decimal Price);

    public record UpdateStockItemDto(long Id, long StockId, long BookId, int Quantity, decimal Price);
}
