using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages;

public partial class Register
{
    private string? Nome { get; set; }
    private string? Usuario { get; set; }
    private string? Email { get; set; }
    private string? Senha { get; set; }
    private string? ConfirmarSenha { get; set; }

    private bool Carregando { get; set; }

    private string? ErroUsuario { get; set; }
    private string? ErroEmail { get; set; }
    private string? ErroSenha { get; set; }
    private string? ErroConfirmarSenha { get; set; }

    private async Task Registrar(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        if (!Validar()) return;

        Carregando = true;

        await Task.Delay(1200); 

        Carregando = false;
        Navigation.NavigateTo("/");
    }

    private void IrParaLogin(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        Navigation.NavigateTo("/");
    }

    private bool Validar()
    {
        ErroUsuario = null;
        ErroEmail = null;
        ErroSenha = null;
        ErroConfirmarSenha = null;

        var valido = true;

        if (string.IsNullOrWhiteSpace(Usuario))
        {
            ErroUsuario = "Usuário é obrigatório.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            ErroEmail = "Informe um e-mail válido.";
            valido = false;
        }

        if (string.IsNullOrWhiteSpace(Senha) || Senha.Length < 8)
        {
            ErroSenha = "A senha deve ter no mínimo 8 caracteres.";
            valido = false;
        }

        if (Senha != ConfirmarSenha)
        {
            ErroConfirmarSenha = "As senhas não coincidem.";
            valido = false;
        }

        return valido;
    }
}