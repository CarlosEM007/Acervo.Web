namespace Acervo.Web.DTOs
{
    public record SellerDto(
        long     Id,
        string   Name,
        string   Email,
        string   Document,
        string?  Phone,
        bool     IsActive,
        DateTime CreatedAt);

    public record CreateSellerDto(string Name, string Email, string Document, string? Phone);

    public record UpdateSellerDto(long Id, string Name, string Email, string? Phone);
}
