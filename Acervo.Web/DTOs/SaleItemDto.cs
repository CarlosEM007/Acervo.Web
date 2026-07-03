namespace Acervo.Web.DTOs
{
    public record SaleItemDto(
        long    Id,
        long    SaleId,
        long    BookId,
        int     Quantity,
        decimal UnitPrice,
        decimal SubTotal);

    public record CreateSaleItemDto(long SaleId, long BookId, int Quantity, decimal UnitPrice);

    public record UpdateSaleItemDto(long Id, long SaleId, long BookId, int Quantity, decimal UnitPrice);
}
