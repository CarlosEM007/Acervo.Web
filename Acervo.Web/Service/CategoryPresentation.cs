namespace Acervo.Web.Service
{
    /// <summary>
    /// Metadados de apresentação das categorias (ícone, descrição, slug). O back-end
    /// só guarda o nome/descrição da categoria; ícone e texto são responsabilidade da UI.
    /// Centralizado aqui para ser reutilizado pelas telas Home e Categorias.
    /// </summary>
    public static class CategoryPresentation
    {
        private record Meta(string Icon, string Description);

        private static readonly Dictionary<string, Meta> Map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Romance"]               = new("fa-solid fa-heart",               "Histórias de amor, relacionamentos e emoções humanas."),
            ["Ficção"]                = new("fa-solid fa-wand-magic-sparkles",  "Narrativas imaginativas que expandem os limites do real."),
            ["Ficção Científica"]     = new("fa-solid fa-rocket",               "Universos distantes, tecnologia e o futuro da humanidade."),
            ["Literatura Brasileira"] = new("fa-solid fa-book-open",            "Clássicos e obras fundamentais da escrita nacional."),
            ["Fantasia"]              = new("fa-solid fa-hat-wizard",           "Mundos mágicos, criaturas extraordinárias e heróis épicos."),
            ["Autoajuda"]             = new("fa-solid fa-star",                 "Desenvolvimento pessoal, produtividade e bem-estar."),
            ["Terror"]                = new("fa-solid fa-ghost",                "Sustos, criaturas e o sobrenatural que arrepia."),
            ["Investigação"]          = new("fa-solid fa-magnifying-glass",     "Mistérios, detetives e enigmas de arrepiar."),
        };

        private static readonly Meta Default =
            new("fa-solid fa-book", "Explore os títulos deste gênero.");

        public static string IconFor(string description) =>
            (Map.GetValueOrDefault(description) ?? Default).Icon;

        public static string DescriptionFor(string description) =>
            (Map.GetValueOrDefault(description) ?? Default).Description;

        public static string Slugify(string value) =>
            value.ToLowerInvariant()
                 .Replace(" ", "-")
                 .Replace("ã", "a").Replace("á", "a").Replace("â", "a")
                 .Replace("ç", "c").Replace("é", "e").Replace("ê", "e")
                 .Replace("í", "i").Replace("ó", "o").Replace("ô", "o")
                 .Replace("ú", "u");
    }
}
