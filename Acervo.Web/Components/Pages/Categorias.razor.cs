using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Categorias
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private record CategoryVm(string Name, string Icon, string Slug, string Description, int Count);

        private List<CategoryVm> Categories { get; } = new()
        {
            new("Ficção Científica", "fa-solid fa-rocket",          "ficcao-cientifica", "Universos distantes, tecnologia e o futuro da humanidade.", 312),
            new("Fantasia",          "fa-solid fa-hat-wizard",      "fantasia",          "Mundos mágicos, criaturas extraordinárias e heróis épicos.", 278),
            new("Romance",           "fa-solid fa-heart",           "romance",           "Histórias de amor, relacionamentos e emoções humanas.", 445),
            new("Suspense",          "fa-solid fa-magnifying-glass","suspense",          "Mistérios, thrillers e narrativas de arrepiar.", 189),
            new("Biografia",         "fa-solid fa-scroll",          "biografia",         "Vidas reais que inspiraram o mundo.", 134),
            new("Autoajuda",         "fa-solid fa-star",            "autoajuda",         "Desenvolvimento pessoal, produtividade e bem-estar.", 221),
            new("História",          "fa-solid fa-landmark",        "historia",          "Do passado ao presente, eventos que moldaram o mundo.", 156),
            new("Literatura",        "fa-solid fa-book",            "literatura",        "Clássicos e obras fundamentais da escrita universal.", 398),
            new("Psicologia",        "fa-solid fa-brain",           "psicologia",        "Comportamento humano, mente e saúde mental.", 112),
            new("Filosofia",         "fa-solid fa-lightbulb",       "filosofia",         "Grandes questões da existência, ética e pensamento.", 98),
            new("Infantil",          "fa-solid fa-balloon",         "infantil",          "Histórias encantadoras para os leitores mais jovens.", 267),
            new("Culinária",         "fa-solid fa-utensils",        "culinaria",         "Receitas, técnicas e a arte da gastronomia.", 143),
        };
    }
}
