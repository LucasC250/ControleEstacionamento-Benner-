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
}
