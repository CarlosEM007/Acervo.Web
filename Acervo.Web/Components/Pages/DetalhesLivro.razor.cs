using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class DetalhesLivro
    {
        [Parameter] public long Id { get; set; }

        [Inject] private NavigationManager Navigation    { get; set; } = default!;
        [Inject] private BookService        BookSvc      { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc    { get; set; } = default!;
        [Inject] private CategoryService    CatSvc       { get; set; } = default!;
        [Inject] private PublisherService   PublisherSvc { get; set; } = default!;
        [Inject] private StockItemService   StockSvc     { get; set; } = default!;
        [Inject] private CartManager        Cart         { get; set; } = default!;
        [Inject] private FavoritesManager   Favorites    { get; set; } = default!;
        [Inject] private ToastService       Toast        { get; set; } = default!;

        // ── Estado ─────────────────────────────────────────────────
        private bool IsLoading    { get; set; } = true;
        private bool IsFavorite   { get; set; } = false;
        private bool DescExpanded { get; set; } = false;

        private record BookDetailVm(
            long     Id,
            string   Title,
            string   Description,
            int      PagesNumber,
            DateTime Release,
            string?  CoverImageUrl,
            string   CategoryName,
            string   CategorySlug,
            long     AuthorId,
            string   AuthorName,
            string   PublisherName,
            decimal  Price,
            decimal? OriginalPrice);

        private BookDetailVm? Book { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            IsLoading = true;
            Book      = null;
            await LoadBook();
            IsLoading = false;
        }

        private async Task LoadBook()
        {
            try
            {
                var bookDto = await BookSvc.GetById(Id);
                if (bookDto is null) return;

                var authTask  = AuthorSvc.GetAll();
                var catTask   = CatSvc.GetAll();
                var pubTask   = PublisherSvc.GetAll();
                var stockTask = StockSvc.GetAll();

                await Task.WhenAll(authTask, catTask, pubTask, stockTask);

                var authorName    = authTask.Result
                    .FirstOrDefault(a => a.Id == bookDto.AuthorId)?.Name ?? "—";
                var category      = catTask.Result
                    .FirstOrDefault(c => c.Id == bookDto.CategoryId);
                var publisherName = pubTask.Result
                    .FirstOrDefault(p => p.Id == bookDto.PublisherId)?.Name ?? "—";
                var price         = stockTask.Result
                    .Where(s => s.BookId == bookDto.Id)
                    .Select(s => s.Price)
                    .DefaultIfEmpty(0m)
                    .Min();

                var slug = Slugify(category?.Description ?? "geral");

                Book = new(
                    bookDto.Id,
                    bookDto.Title,
                    bookDto.Description,
                    bookDto.PagesNumber,
                    bookDto.Release,
                    string.IsNullOrEmpty(bookDto.CoverImageUrl) ? null : bookDto.CoverImageUrl,
                    category?.Description ?? "—",
                    slug,
                    bookDto.AuthorId,
                    authorName,
                    publisherName,
                    price,
                    null);

                // Reflete o estado real de favorito do usuário logado.
                var favItem = await Favorites.FindItem(bookDto.Id);
                IsFavorite = favItem is not null;
            }
            catch
            {
                // API offline — Book fica null → template mostra "não encontrado"
                Toast.ShowError("Não foi possível carregar o livro.");
            }
        }

        private async Task AddToCart()
        {
            if (Book is null) return;

            try
            {
                if (await Cart.AddBook(Book.Id, Book.Price))
                    Toast.ShowSuccess($"«{Book.Title}» adicionado ao carrinho!");
                else
                    Toast.ShowError("Não foi possível adicionar ao carrinho.");
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private async Task ToggleFavorite()
        {
            if (Book is null) return;

            try
            {
                if (IsFavorite)
                {
                    if (await Favorites.Remove(Book.Id))
                    {
                        IsFavorite = false;
                        Toast.ShowSuccess($"«{Book.Title}» removido dos favoritos.");
                    }
                    else
                    {
                        Toast.ShowError("Não foi possível remover dos favoritos.");
                    }
                }
                else
                {
                    if (await Favorites.Add(Book.Id))
                    {
                        IsFavorite = true;
                        Toast.ShowSuccess($"«{Book.Title}» adicionado aos favoritos!");
                    }
                    else
                    {
                        Toast.ShowError("Não foi possível adicionar aos favoritos.");
                    }
                }
            }
            catch
            {
                Toast.ShowError("Erro ao comunicar com o servidor.");
            }
        }

        private static string Slugify(string value) =>
            value.ToLowerInvariant()
                 .Replace(" ", "-")
                 .Replace("ã", "a").Replace("á", "a").Replace("â", "a")
                 .Replace("ç", "c").Replace("é", "e").Replace("ê", "e")
                 .Replace("í", "i").Replace("ó", "o").Replace("ô", "o")
                 .Replace("ú", "u");
    }
}
