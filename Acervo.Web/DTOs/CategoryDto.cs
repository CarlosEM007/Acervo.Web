namespace Acervo.Web.DTOs
{
    public record CategoryDto(long Id, string Description);

    public record CreateCategoryDto(string Description);

    public record UpdateCategoryDto(long Id, string Description);
}
