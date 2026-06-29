using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Favoritos
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private record FavItemVm(long BookId, string Title, string AuthorName,
            string CategoryName, decimal Price, string? CoverImageUrl);

        private List<FavItemVm> Items { get; set; } = new()
        {
            new(1, "Fundação", "Isaac Asimov", "Ficção Científica", 39.90m, null),
            new(3, "Duna", "Frank Herbert", "Ficção Científica", 49.90m, null),
            new(7, "Cem Anos de Solidão", "Gabriel García Márquez", "Romance", 44.90m, null),
        };

        private void RemoveFavorite(FavItemVm item) => Items.Remove(item);

        private void AddToCart(FavItemVm item)
        {
            // Substituir pela lógica de carrinho real
            Navigation.NavigateTo($"/livro/{item.BookId}");
        }
    }
}
