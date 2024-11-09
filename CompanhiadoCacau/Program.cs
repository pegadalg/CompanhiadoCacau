using CompanhiadoCacau.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adiciona o serviço HttpClient para permitir chamadas HTTP
builder.Services.AddHttpClient("ViaCepClient", client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/"); // Definindo a URL base para as chamadas HTTP
    client.Timeout = TimeSpan.FromSeconds(30); // Ajustando o tempo de timeout (opcional)
});

// Configura a conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("CompanhiadoCacauConnection");
builder.Services.AddDbContext<CiadoCacauContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS (HTTP Strict Transport Security) para segurança adicional
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
