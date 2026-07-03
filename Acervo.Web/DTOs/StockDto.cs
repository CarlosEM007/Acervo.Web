namespace Acervo.Web.DTOs
{
    public record StockDto(long Id, long SellerId, DateTime UpdatedAt);

    public record CreateStockDto(long SellerId);

    public record UpdateStockDto(long Id, long SellerId);
}
