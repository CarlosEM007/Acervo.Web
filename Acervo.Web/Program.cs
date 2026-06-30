using Acervo.Web.Components;
using Acervo.Web.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SessionService>();

var apiBase = new Uri("https://localhost:7104/api/");

void ConfigureClient(HttpClient c) => c.BaseAddress = apiBase;

if (builder.Environment.IsDevelopment())
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    builder.Services.AddHttpClient<UserService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
    builder.Services.AddHttpClient<BookService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
    builder.Services.AddHttpClient<AuthorService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
    builder.Services.AddHttpClient<CategoryService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
    builder.Services.AddHttpClient<PublisherService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
    builder.Services.AddHttpClient<StockItemService>(ConfigureClient).ConfigurePrimaryHttpMessageHandler(() => handler);
}
else
{
    builder.Services.AddHttpClient<UserService>(ConfigureClient);
    builder.Services.AddHttpClient<BookService>(ConfigureClient);
    builder.Services.AddHttpClient<AuthorService>(ConfigureClient);
    builder.Services.AddHttpClient<CategoryService>(ConfigureClient);
    builder.Services.AddHttpClient<PublisherService>(ConfigureClient);
    builder.Services.AddHttpClient<StockItemService>(ConfigureClient);
}

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
