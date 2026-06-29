using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Sobre
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private record StatItem(string Value, string Label);
        private record PillarItem(string Icon, string Title, string Text);
        private record TimelineEvent(string Year, string Title, string Description);

        private List<StatItem> Stats { get; } = new()
        {
            new("12k+",  "Títulos disponíveis"),
            new("850+",  "Autores catalogados"),
            new("40k+",  "Leitores ativos"),
            new("4.8★",  "Avaliação média"),
        };

        private List<PillarItem> Pillars { get; } = new()
        {
            new("fa-solid fa-book-open", "Missão",
                "Democratizar o acesso à leitura, oferecendo um acervo vasto e acessível para leitores de todos os perfis."),
            new("fa-solid fa-star", "Visão",
                "Ser a principal plataforma de livros digitais da América Latina, reconhecida pela qualidade e curadoria."),
            new("fa-solid fa-heart", "Valores",
                "Paixão por livros, respeito ao leitor, diversidade de vozes e compromisso com a cultura literária."),
        };

        private List<TimelineEvent> Timeline { get; } = new()
        {
            new("2020", "Fundação do Acervo",
                "Nascemos como um pequeno projeto com a missão de digitalizar a experiência de livraria independente."),
            new("2021", "Primeiros 1.000 títulos",
                "Alcançamos a marca de mil livros catalogados e firmamos parcerias com editoras nacionais."),
            new("2022", "Plataforma reformulada",
                "Lançamos a versão 2.0 com nova interface, sistema de recomendações e clube de leitura."),
            new("2023", "40 mil leitores",
                "A comunidade Acervo chegou a 40 mil leitores ativos em todo o Brasil."),
            new("2024", "Expansão internacional",
                "Iniciamos operações em Portugal e nos países lusófonos da América do Sul."),
        };
    }
}
