using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Acervo.Web.Components.Pages
{
    public partial class Busca
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private BookService        BookSvc   { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc { get; set; } = default!;
        [Inject] private CategoryService    CatSvc    { get; set; } = default!;
        [Inject] private StockItemService   StockSvc  { get; set; } = default!;

        private record BookVm(long Id, string Title, string AuthorName, string CategoryName, decimal Price);

        private string       Query    { get; set; } = string.Empty;
        private bool         IsLoading { get; set; } = true;

        private List<BookVm> AllBooks { get; set; } = [];
        private List<BookVm> Results  { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            // Ler query string antes de carregar os dados
            var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("q", out var q))
                Query = q!;

            try
            {
                var booksTask  = BookSvc.GetAll();
                var authTask   = AuthorSvc.GetAll();
                var catTask    = CatSvc.GetAll();
                var stockTask  = StockSvc.GetAll();

                await Task.WhenAll(booksTask, authTask, catTask, stockTask);

                var authors    = authTask.Result.ToDictionary(a => a.Id, a => a.Name);
                var categories = catTask.Result.ToDictionary(c => c.Id, c => c.Description);
                var prices     = stockTask.Result
                                    .GroupBy(s => s.BookId)
                                    .ToDictionary(g => g.Key, g => g.Min(s => s.Price));

                AllBooks = booksTask.Result
                    .Select(b => new BookVm(
                        b.Id,
                        b.Title,
                        authors.GetValueOrDefault(b.AuthorId, "—"),
                        categories.GetValueOrDefault(b.CategoryId, "—"),
                        prices.GetValueOrDefault(b.Id, 0m)))
                    .ToList();
            }
            catch { /* API offline */ }
            finally
            {
                IsLoading = false;
                RunSearch();
            }
        }

        private void RunSearch()
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                Results.Clear();
                return;
            }

            var term = Query.Trim().ToLowerInvariant();
            Results = AllBooks
                .Where(b =>
                    b.Title.Contains(term, StringComparison.OrdinalIgnoreCase)        ||
                    b.AuthorName.Contains(term, StringComparison.OrdinalIgnoreCase)   ||
                    b.CategoryName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
