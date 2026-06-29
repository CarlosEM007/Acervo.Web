namespace Acervo.Web.DTOs
{
    public record StockItemDto(long Id, long BookId, decimal Price, int Quantity);
}
