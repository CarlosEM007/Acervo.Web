using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Acervo.Web.Components.Pages
{
    public partial class Login
    {
        [Inject] private UserService  _service { get; set; } = default!;
        [Inject] private ToastService Toast    { get; set; } = default!;

        private string? Usuario { get; set; }
        private string? Senha { get; set; }

        private async Task Acessar()
        {
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Senha))
            {
                Toast.ShowError("Informe usuário e senha.");
                return;
            }

            var ok = await _service.LoginUser(Usuario, Senha);

            if (ok)
            {
                Toast.ShowSuccess("Bem-vindo!");
                Navigation.NavigateTo("/Home");
            }
            else
            {
                Toast.ShowError("Não foi possível entrar. Verifique suas credenciais.");
            }
        }

        private void Registrar(MouseEventArgs e)
        {
            Navigation.NavigateTo("/registrar");
        }
    }
}
