using Acervo.Web.Components;
using Acervo.Web.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SessionService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<CartManager>();
builder.Services.AddScoped<FavoritesManager>();

var apiBase = new Uri("https://localhost:7104/api/");
var isDevelopment = builder.Environment.IsDevelopment();

// Handler que injeta o JWT (Authorization: Bearer) em cada requisição à API.
builder.Services.AddTransient<AuthTokenHandler>();

// Registra um HttpClient tipado apontando para a API. Anexa o token JWT e, em
// desenvolvimento, ignora a validação do certificado self-signed do Kestrel/localhost.
void AddApiClient<TService>() where TService : class
{
    var clientBuilder = builder.Services.AddHttpClient<TService>(c => c.BaseAddress = apiBase)
        .AddHttpMessageHandler<AuthTokenHandler>();

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
