using Acervo.Web.Components;
using Acervo.Web.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SessionService>();

var apiBase = new Uri("https://localhost:7104/api/");
var isDevelopment = builder.Environment.IsDevelopment();

// Registra um HttpClient tipado apontando para a API. Em desenvolvimento,
// ignora a validação do certificado self-signed do Kestrel/localhost.
void AddApiClient<TService>() where TService : class
{
    var clientBuilder = builder.Services.AddHttpClient<TService>(c => c.BaseAddress = apiBase);

    if (isDevelopment)
    {
        clientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    }
}

AddApiClient<UserService>();
AddApiClient<BookService>();
AddApiClient<AuthorService>();
AddApiClient<CategoryService>();
AddApiClient<PublisherService>();
AddApiClient<SellerService>();
AddApiClient<CartService>();
AddApiClient<CartItemService>();
AddApiClient<FavoritesService>();
AddApiClient<FavoritesItemService>();
AddApiClient<LibraryService>();
AddApiClient<LibraryItemService>();
AddApiClient<SaleService>();
AddApiClient<SaleItemService>();
AddApiClient<StockService>();
AddApiClient<StockItemService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
