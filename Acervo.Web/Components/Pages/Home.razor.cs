using Acervo.Web.DTOs;
using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Home
    {
        [Inject] private NavigationManager Navigation  { get; set; } = default!;
        [Inject] private BookService        BookSvc    { get; set; } = default!;
        [Inject] private AuthorService      AuthorSvc  { get; set; } = default!;
        [Inject] private CategoryService    CatSvc     { get; set; } = default!;
        [Inject] private StockItemService   StockSvc   { get; set; } = default!;

        private record BookVm(long Id, string Title, string AuthorName, string CategoryName,
            decimal Price, decimal? OriginalPrice, string? CoverImageUrl);

        private record CategoryVm(string Name, string Icon, string Slug, int Count);

        private bool IsLoading { get; set; } = true;

        private List<CategoryVm> FeaturedCategories { get; } = new()
        {
            new("Ficção Científica", "fa-solid fa-rocket",            "ficcao-cientifica", 0),
            new("Fantasia",          "fa-solid fa-hat-wizard",        "fantasia",          0),
            new("Romance",           "fa-solid fa-heart",             "romance",           0),
            new("Suspense",          "fa-solid fa-magnifying-glass",  "suspense",          0),
            new("Biografia",         "fa-solid fa-scroll",            "biografia",         0),
            new("Autoajuda",         "fa-solid fa-star",              "autoajuda",         0),
            new("História",          "fa-solid fa-landmark",          "historia",          0),
            new("Literatura",        "fa-solid fa-book",              "literatura",        0),
        };

        private List<BookVm> NewReleases  { get; set; } = [];
        private List<BookVm> BestSellers  { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var booksTask  = BookSvc.GetAll();
                var authTask   = AuthorSvc.GetAll();
                var catTask    = CatSvc.GetAll();
                var stockTask  = StockSvc.GetAll();

                await Task.WhenAll(booksTask, authTask, catTask, stockTask);

                var books      = booksTask.Result;
                var authors    = authTask.Result.ToDictionary(a => a.Id, a => a.Name);
                var categories = catTask.Result.ToDictionary(c => c.Id, c => c.Description);
                var prices     = stockTask.Result
                                    .GroupBy(s => s.BookId)
                                    .ToDictionary(g => g.Key, g => g.Min(s => s.Price));

                BookVm ToVm(BookDto b) => new(
                    b.Id,
                    b.Title,
                    authors.GetValueOrDefault(b.AuthorId, "—"),
                    categories.GetValueOrDefault(b.CategoryId, "—"),
                    prices.GetValueOrDefault(b.Id, 0m),
                    null,
                    string.IsNullOrEmpty(b.CoverImageUrl) ? null : b.CoverImageUrl);

                // Lançamentos = 4 mais recentes
                NewReleases = books
                    .OrderByDescending(b => b.Release)
                    .Take(4)
                    .Select(ToVm)
                    .ToList();

                // Mais vendidos = 4 seguintes (ordem aleatória estável)
                BestSellers = books
                    .OrderBy(b => b.Id)
                    .Skip(4)
                    .Take(4)
                    .Select(ToVm)
                    .ToList();
            }
            catch { /* API offline — listas ficam vazias */ }
            finally { IsLoading = false; }
        }
    }
}
