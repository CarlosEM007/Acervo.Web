using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Favoritos
    {
        [Inject] private NavigationManager     Navigation { get; set; } = default!;
        [Inject] private SessionService        Session    { get; set; } = default!;
        [Inject] private FavoritesService      FavSvc     { get; set; } = default!;
        [Inject] private FavoritesItemService  FavItemSvc { get; set; } = default!;
        [Inject] private BookService           BookSvc    { get; set; } = default!;
        [Inject] private AuthorService         AuthorSvc  { get; set; } = default!;
        [Inject] private CategoryService       CatSvc     { get; set; } = default!;
        [Inject] private StockItemService      StockSvc   { get; set; } = default!;
        [Inject] private CartManager           Cart       { get; set; } = default!;
        [Inject] private ToastService          Toast      { get; set; } = default!;

        private record FavItemVm(long FavoritesItemId, long BookId, string Title, string AuthorName,
            string CategoryName, decimal Price, string? CoverImageUrl);

        private bool IsLoading { get; set; } = true;
        private List<FavItemVm> Items { get; set; } = [];

        protected override async Task OnInitializedAsync() => await LoadFavorites();

        private async Task LoadFavorites()
        {
            IsLoading = true;

            if (Session.UserId is not long userId)
            {
                IsLoading = false;
                Toast.ShowError("Faça login para ver seus favoritos.");
                return;
            }

            try
            {
                var favorites = await FavSvc.GetByUser(userId);
                if (favorites is null)
                {
                    Items = [];
                    return;
                }

                var itemsTask = FavItemSvc.GetAll();
                var booksTask = BookSvc.GetAll();
                var authTask  = AuthorSvc.GetAll();
                var catTask   = CatSvc.GetAll();
                var stockTask = StockSvc.GetAll();

                await Task.WhenAll(itemsTask, booksTask, authTask, catTask, stockTask);

                var books   = booksTask.Result.ToDictionary(b => b.Id);
                var authors = authTask.Result.ToDictionary(a => a.Id, a => a.Name);
                var cats    = catTask.Result.ToDictionary(c => c.Id, c => c.Description);
                var prices  = stockTask.Result
                    .GroupBy(s => s.BookId)
                    .ToDictionary(g => g.Key, g => g.Min(s => s.Price));

                Items = itemsTask.Result
                    .Where(fi => fi.FavoritesId == favorites.Id)
                    .Select(fi =>
                    {
                        books.TryGetValue(fi.BookId, out var book);
                        return new FavItemVm(
                            fi.Id,
                            fi.BookId,
                            book?.Title ?? "—",
                            book is null ? "—" : authors.GetValueOrDefault(book.AuthorId, "—"),
                            book is null ? "—" : cats.GetValueOrDefault(book.CategoryId, "—"),
                            prices.GetValueOrDefault(fi.BookId, 0m),
                            string.IsNullOrEmpty(book?.CoverImageUrl) ? null : book!.CoverImageUrl);
                    })
                    .ToList();
            }
            catch
            {
                Toast.ShowError("Não foi possível carregar os favoritos.");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RemoveFavorite(FavItemVm item)
        {
            try
            {
                if (await FavItemSvc.Delete(item.FavoritesItemId))
                {
                    Items.Remove(item);
                    Toast.ShowSuccess($"«{item.Title}» removido dos favoritos.");
                }
                else
                {
                    Toast.ShowError("Não foi possível remover o favorito.");
                }
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private async Task AddToCart(FavItemVm item)
        {
            try
            {
                if (await Cart.AddBook(item.BookId, item.Price))
                    Toast.ShowSuccess($"«{item.Title}» adicionado ao carrinho!");
                else
                    Toast.ShowError("Não foi possível adicionar ao carrinho.");
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }
    }
}
