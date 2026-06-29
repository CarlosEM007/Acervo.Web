using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Acervo.Web.Components.Shared;

public class AppButtonBase : ComponentBase
{
    // ── Identificação ──────────────────────────────────────────────
    [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];

    // ── Conteúdo ───────────────────────────────────────────────────
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public RenderFragment? LeadingIcon { get; set; }
    [Parameter] public RenderFragment? TrailingIcon { get; set; }

    // ── Tipo HTML ─────────────────────────────────────────────────
    /// <summary>button | submit | reset</summary>
    [Parameter] public string Type { get; set; } = "button";

    // ── Variante visual ───────────────────────────────────────────
    /// <summary>primary | secondary | outline | ghost | danger</summary>
    [Parameter] public string Variant { get; set; } = "primary";

    // ── Cor customizada ───────────────────────────────────────────
    /// <summary>
    /// Cor de fundo personalizada. Qualquer valor CSS válido.
    /// Ex: "#3b82f6", "rgb(16,185,129)", "hsl(271,91%,65%)"
    /// Quando definida, sobrescreve o Variant.
    /// </summary>
    [Parameter] public string? Color { get; set; }

    /// <summary>Cor do texto quando Color está definida. Padrão: #ffffff</summary>
    [Parameter] public string? TextColor { get; set; }

    // ── Tamanho ───────────────────────────────────────────────────
    /// <summary>sm | md | lg</summary>
    [Parameter] public string Size { get; set; } = "md";

    // ── Dimensões personalizadas ──────────────────────────────────
    /// <summary>Largura customizada. Ex: "200px", "100%", "12rem"</summary>
    [Parameter] public string? Width { get; set; }

    /// <summary>Altura customizada. Ex: "48px", "3rem"</summary>
    [Parameter] public string? Height { get; set; }

    // ── Flags de estado ───────────────────────────────────────────
    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool FullWidth { get; set; }
    [Parameter] public bool Loading { get; set; }

    /// <summary>Texto exibido enquanto Loading = true</summary>
    [Parameter] public string? LoadingText { get; set; }

    // ── Callback ──────────────────────────────────────────────────
    [Parameter] public EventCallback<MouseEventArgs> OnClickCallback { get; set; }

    // ── Classe computada ──────────────────────────────────────────
    protected string ButtonClass
    {
        get
        {
            var variant = string.IsNullOrWhiteSpace(Color) ? Variant : "custom";
            var parts = new List<string>
            {
                "app-btn",
                $"app-btn--{variant}",
                $"app-btn--{Size}",
            };
            if (FullWidth) parts.Add("app-btn--full");
            if (Loading) parts.Add("app-btn--loading");
            return string.Join(" ", parts);
        }
    }

    // ── Estilo computado ──────────────────────────────────────────
    protected string ButtonStyle
    {
        get
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(Width)) parts.Add($"width:{Width}");
            if (!string.IsNullOrWhiteSpace(Height)) parts.Add($"height:{Height}");

            // Injeta as CSS vars para a variante custom
            if (!string.IsNullOrWhiteSpace(Color))
            {
                parts.Add($"--btn-custom-bg:{Color}");

                if (!string.IsNullOrWhiteSpace(TextColor))
                    parts.Add($"--btn-custom-text:{TextColor}");

                // Sombra automática com 30% de opacidade da cor (fallback genérico)
                parts.Add($"--btn-custom-shadow:{Color}4D");
            }

            return parts.Count > 0 ? string.Join(";", parts) + ";" : string.Empty;
        }
    }

    // ── Handler ───────────────────────────────────────────────────
    protected async Task HandleClick(MouseEventArgs e)
    {
        if (Disabled || Loading) return;
        if (OnClickCallback.HasDelegate)
            await OnClickCallback.InvokeAsync(e);
    }
}