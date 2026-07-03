using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Categorias
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private CategoryService   CatSvc     { get; set; } = default!;
        [Inject] private BookService       BookSvc    { get; set; } = default!;

        private record CategoryVm(string Name, string Icon, string Slug, string Description, int Count);

        private bool IsLoading { get; set; } = true;
        private List<CategoryVm> Categories { get; set; } = [];

        // Metadados de apresentação (ícone/descrição) por categoria — o back-end
        // só armazena a descrição/nome, então ícone e texto são responsabilidade da UI.
        private record CategoryMeta(string Icon, string Description);

        private static readonly Dictionary<string, CategoryMeta> Meta = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Romance"]               = new("fa-solid fa-heart",            "Histórias de amor, relacionamentos e emoções humanas."),
            ["Ficção"]                = new("fa-solid fa-wand-magic-sparkles", "Narrativas imaginativas que expandem os limites do real."),
            ["Ficção Científica"]     = new("fa-solid fa-rocket",           "Universos distantes, tecnologia e o futuro da humanidade."),
            ["Literatura Brasileira"] = new("fa-solid fa-book-open",        "Clássicos e obras fundamentais da escrita nacional."),
            ["Fantasia"]              = new("fa-solid fa-hat-wizard",       "Mundos mágicos, criaturas extraordinárias e heróis épicos."),
            ["Autoajuda"]             = new("fa-solid fa-star",             "Desenvolvimento pessoal, produtividade e bem-estar."),
            ["Terror"]                = new("fa-solid fa-ghost",            "Sustos, criaturas e o sobrenatural que arrepia."),
            ["Investigação"]          = new("fa-solid fa-magnifying-glass", "Mistérios, detetives e enigmas de arrepiar."),
        };

        private static readonly CategoryMeta DefaultMeta =
            new("fa-solid fa-book", "Explore os títulos deste gênero.");

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var catTask  = CatSvc.GetAll();
                var bookTask = BookSvc.GetAll();
                await Task.WhenAll(catTask, bookTask);

                var bookCount = bookTask.Result
                    .GroupBy(b => b.CategoryId)
                    .ToDictionary(g => g.Key, g => g.Count());

                Categories = catTask.Result
                    .Select(c =>
                    {
                        var meta = Meta.GetValueOrDefault(c.Description, DefaultMeta);
                        return new CategoryVm(
                            c.Description,
                            meta.Icon,
                            Slugify(c.Description),
                            meta.Description,
                            bookCount.GetValueOrDefault(c.Id, 0));
                    })
                    .OrderByDescending(c => c.Count)
                    .ToList();
            }
            catch { /* API offline — lista fica vazia */ }
            finally { IsLoading = false; }
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
