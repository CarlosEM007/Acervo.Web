using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages
{
    public partial class Contato
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private string? ContactName { get; set; }
        private string? ContactEmail { get; set; }
        private string? ContactSubject { get; set; }
        private string? ContactMessage { get; set; }
        private string? FormError { get; set; }
        private bool IsSending { get; set; }
        private bool Sent { get; set; }

        private record FaqItem(string Q, string A);

        private List<FaqItem> Faqs { get; } = new()
        {
            new("Como faço para ler um livro comprado?",
                "Após a compra, o livro fica disponível em Minha Biblioteca. Você pode ler direto no navegador."),
            new("Posso pedir reembolso?",
                "Sim, em até 7 dias após a compra, caso o livro não tenha sido acessado."),
            new("Consigo ler em vários dispositivos?",
                "Sim! Sua biblioteca fica disponível em qualquer dispositivo com acesso ao Acervo."),
        };

        private async Task SendMessage()
        {
            FormError = null;

            if (string.IsNullOrWhiteSpace(ContactName) || string.IsNullOrWhiteSpace(ContactEmail)
                || string.IsNullOrWhiteSpace(ContactMessage))
            {
                FormError = "Preencha todos os campos obrigatórios.";
                return;
            }

            IsSending = true;
            await Task.Delay(800);
            IsSending = false;
            Sent = true;
        }
    }
}
