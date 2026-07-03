using Acervo.Web.DTOs;
using Acervo.Web.Service;
using Microsoft.AspNetCore.Components;

namespace Acervo.Web.Components.Pages;

public partial class Register
{
    [Inject] private UserService _service { get; set; } = default!;

    private string? Nome { get; set; }
    private string? Usuario { get; set; }
    private string? Email { get; set; }
    private string? Senha { get; set; }
    private string? ConfirmarSenha { get; set; }

    private bool Carregando { get; set; }

    private string? ErroNome { get; set; }
    private string? ErroEmail { get; set; }
    private string? ErroSenha { get; set; }
    private string? ErroConfirmarSenha { get; set; }
    private string? ErroGeral { get; set; }

    private async Task Registrar(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        ErroGeral = null;

        if (!Validar()) return;

        Carregando = true;

        try
        {
            var dto = new CreateUserDto(Nome!, Email!, Senha!);
            var ok  = await _service.Create(dto);

            if (ok)
                Navigation.NavigateTo("/");
            else
                ErroGeral = "Não foi possível concluir o cadastro. Verifique os dados e tente novamente.";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Register] Registrar error: {ex.Message}");
            ErroGeral = "Não foi possível conectar ao servidor. Tente novamente mais tarde.";
        }
        finally
        {
            Carregando = false;
        }
    }

    private void IrParaLogin(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        Navigation.NavigateTo("/");
    }

    private bool Validar()
    {
        ErroNome = null;
        ErroEmail = null;
        ErroSenha = null;
        ErroConfirmarSenha = null;

        var valido = true;

        if (string.IsNullOrWhiteSpace(Nome))
        {
            ErroNome = "Nome é obrigatório.";
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
