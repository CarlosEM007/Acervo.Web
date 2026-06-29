using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Acervo.Web.Components.Shared
{
    public class AppInputBase : ComponentBase
    {
        // ── Identificação ──────────────────────────────────────────────
        [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N")[..8];

        // ── Conteúdo ───────────────────────────────────────────────────
        [Parameter] public string? Label { get; set; }
        [Parameter] public string? Placeholder { get; set; }
        [Parameter] public string? HelperText { get; set; }
        [Parameter] public string? ErrorMessage { get; set; }

        // ── Valor ──────────────────────────────────────────────────────
        [Parameter] public string? Value { get; set; }
        [Parameter] public EventCallback<string?> ValueChanged { get; set; }

        // ── Tipo ──────────────────────────────────────────────────────
        /// <summary>text | password | email | number | tel | url | search | date</summary>
        [Parameter] public string Type { get; set; } = "text";

        // ── Ícones ─────────────────────────────────────────────────────
        [Parameter] public RenderFragment? LeadingIcon { get; set; }
        [Parameter] public RenderFragment? TrailingIcon { get; set; }

        // ── Restrições ────────────────────────────────────────────────
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public int MaxLength { get; set; }

        // ── Dimensões ─────────────────────────────────────────────────
        /// <summary>Largura do componente. Ex: "320px", "100%", "20rem"</summary>
        [Parameter] public string? Width { get; set; }

        /// <summary>Altura do campo de input. Ex: "40px", "3rem"</summary>
        [Parameter] public string? Height { get; set; }

        // ── Callbacks adicionais ──────────────────────────────────────
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        // ── Estado interno ────────────────────────────────────────────
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);

        // ── Estilos computados ────────────────────────────────────────
        protected string WrapperStyle =>
            string.IsNullOrWhiteSpace(Width) ? string.Empty : $"width:{Width};";

        protected string InputStyle =>
            string.IsNullOrWhiteSpace(Height) ? string.Empty : $"height:{Height};";

        // ── Handlers ──────────────────────────────────────────────────
        protected async Task HandleInput(ChangeEventArgs e)
        {
            Value = e.Value?.ToString();
            await ValueChanged.InvokeAsync(Value);
        }

        protected async Task HandleBlur(FocusEventArgs e)
        {
            if (OnBlur.HasDelegate)
                await OnBlur.InvokeAsync(e);
        }

        protected async Task HandleFocus(FocusEventArgs e)
        {
            if (OnFocus.HasDelegate)
                await OnFocus.InvokeAsync(e);
        }
    }
}
