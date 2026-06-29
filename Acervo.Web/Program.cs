using Acervo.Web.Components;
using Acervo.Web.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SessionService>();

var apiBase = new Uri("https://localhost:7001/api/");

builder.Services.AddHttpClient<UserService>(c       => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<BookService>(c       => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<AuthorService>(c     => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<CategoryService>(c   => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<PublisherService>(c  => c.BaseAddress = apiBase);
builder.Services.AddHttpClient<StockItemService>(c  => c.BaseAddress = apiBase);

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
