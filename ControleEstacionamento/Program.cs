using ControleEstacionamento.Data;
using ControleEstacionamento.DAOs;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Services;
using ControleEstacionamento.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "A connection string 'DefaultConnection' não foi configurada. " +
        "Configure-a com User Secrets ou pela variável de ambiente ConnectionStrings__DefaultConnection.");
}

builder.Services.AddDbContext<EstacionamentoContext>(options =>
    options.UseSqlServer(connectionString, sqlServer => sqlServer.EnableRetryOnFailure())
);


builder.Services.AddScoped<IEstacionamentoDAO, EstacionamentoDAO>();

builder.Services.AddScoped<IEntradaService, EntradaService>();
builder.Services.AddScoped<ISaidaService, SaidaService>();
builder.Services.AddScoped<ITabelaPrecoService, TabelaPrecoService>();
builder.Services.AddScoped<IConsultaEstacionamentoService, ConsultaEstacionamentoService>();
builder.Services.AddScoped<ICalculadoraPrecoService, CalculadoraPrecoService>();
builder.Services.AddSingleton(TimeProvider.System);

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
