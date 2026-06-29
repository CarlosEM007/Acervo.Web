using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Acervo.Web.Components.Pages
{
    public partial class Login
    {
        [Inject] private UserService _service { get; set; } = default!;

        private string? Usuario { get; set; }
        private string? Senha { get; set; }

        private async Task Acessar()
        {
            var ok = await _service.LoginUser(Usuario, Senha);

            if(ok)
                Navigation.NavigateTo("/Home");
        }

        private void Registrar(MouseEventArgs e)
        {
            Navigation.NavigateTo("/registrar");
        }
    }
}
