namespace Acervo.Web.DTOs
{
    public record SaleDto(
        long      Id,
        long      UserId,
        long      SellerId,
        int       Status,
        decimal   TotalAmount,
        DateTime  CreatedAt,
        DateTime? CompletedAt);

    public record CreateSaleDto(long UserId, long SellerId);

    public record UpdateSaleDto(long Id, long UserId, long SellerId);
}
