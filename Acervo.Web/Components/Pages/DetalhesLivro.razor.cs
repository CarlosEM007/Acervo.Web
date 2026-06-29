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

        // ── Estado ─────────────────────────────────────────────────
        private bool    IsLoading      { get; set; } = true;
        private bool    IsFavorite     { get; set; } = false;
        private bool    DescExpanded   { get; set; } = false;
        private string? FeedbackMessage { get; set; }
        private bool    FeedbackSuccess { get; set; }

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

                // Slug simples: sem acentos/espaços
                var slug = category?.Description
                    .ToLowerInvariant()
                    .Replace(" ", "-")
                    .Replace("ã", "a").Replace("á", "a").Replace("â", "a")
                    .Replace("ç", "c").Replace("é", "e").Replace("ê", "e")
                    .Replace("í", "i").Replace("ó", "o").Replace("ô", "o")
                    .Replace("ú", "u") ?? "geral";

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
            }
            catch { /* API offline — Book fica null → template mostra "não encontrado" */ }
        }

        private async Task AddToCart()
        {
            if (Book is null) return;

            // TODO: integrar com CartService quando houver contexto de usuário
            await Task.Delay(200);

            FeedbackMessage = $"«{Book.Title}» adicionado ao carrinho!";
            FeedbackSuccess = true;
            StateHasChanged();

            await Task.Delay(3000);
            FeedbackMessage = null;
            StateHasChanged();
        }

        private async Task ToggleFavorite()
        {
            if (Book is null) return;

            // TODO: integrar com FavoritesService quando houver contexto de usuário
            await Task.Delay(150);

            IsFavorite      = !IsFavorite;
            FeedbackMessage = IsFavorite
                ? $"«{Book.Title}» adicionado aos favoritos!"
                : $"«{Book.Title}» removido dos favoritos.";
            FeedbackSuccess = IsFavorite;
            StateHasChanged();

            await Task.Delay(3000);
            FeedbackMessage = null;
            StateHasChanged();
        }
    }
}
