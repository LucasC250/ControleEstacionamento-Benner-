using ControleEstacionamento.Data;
using ControleEstacionamento.DAOs;
using ControleEstacionamento.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var caminhoBanco = Path.Combine(
    builder.Environment.ContentRootPath,
    "estacionamento.db"
);

builder.Services.AddDbContext<EstacionamentoContext>(options =>
    options.UseSqlite($"Data Source={caminhoBanco}")
);

builder.Services.AddScoped<EstacionamentoDAO>();
builder.Services.AddScoped<EstacionamentoService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EstacionamentoContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();