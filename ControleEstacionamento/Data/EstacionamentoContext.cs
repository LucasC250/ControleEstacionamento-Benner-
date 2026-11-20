using ControleEstacionamento.EstacionamentoApp;
using Microsoft.EntityFrameworkCore;

namespace ControleEstacionamento.Data;

public class EstacionamentoContext : DbContext
{
    public EstacionamentoContext(DbContextOptions<EstacionamentoContext> options)
        : base(options)
    {
    }

    public DbSet<TabelaPreco> TabelasPreco => Set<TabelaPreco>();
    public DbSet<RegistroEstacionamento> Registros => Set<RegistroEstacionamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TabelaPreco>(entity =>
        {
            entity.Property(t => t.ValorHoraInicial).HasPrecision(10, 2);
            entity.Property(t => t.ValorHoraAdicional).HasPrecision(10, 2);
            entity.HasIndex(t => new { t.DataInicioVigencia, t.DataFimVigencia });
        });

        modelBuilder.Entity<RegistroEstacionamento>(entity =>
        {
            entity.Property(r => r.Placa).HasMaxLength(7).IsRequired();
            entity.Property(r => r.ValorPago).HasPrecision(10, 2);

            entity.HasIndex(r => r.Placa)
                .IsUnique()
                .HasFilter("[HorarioSaida] IS NULL");
        });
    }
}
