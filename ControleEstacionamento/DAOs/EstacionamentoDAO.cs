using ControleEstacionamento.Data;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.EstacionamentoApp;
using Microsoft.EntityFrameworkCore;

namespace ControleEstacionamento.DAOs;

public class EstacionamentoDAO : IEstacionamentoDAO
{
    private readonly EstacionamentoContext _db;

    public EstacionamentoDAO(EstacionamentoContext db)
    {
        _db = db;
    }

    public bool ExisteEntradaAberta(string placa)
    {
        return _db.Registros.Any(r =>
            r.Placa == placa && r.HorarioSaida == null);
    }

    public bool ExisteTabelaVigente(DateTime data)
    {
        return _db.TabelasPreco.Any(t =>
            t.DataInicioVigencia <= data.Date &&
            t.DataFimVigencia >= data.Date);
    }

    public void AdicionarRegistro(RegistroEstacionamento registro)
    {
        _db.Registros.Add(registro);
        _db.SaveChanges();
    }

    public RegistroEstacionamento? ObterEntradaAberta(string placa)
    {
        return _db.Registros
            .Where(r => r.Placa == placa && r.HorarioSaida == null)
            .OrderByDescending(r => r.HorarioEntrada)
            .FirstOrDefault();
    }

    public TabelaPreco? ObterTabelaPorData(DateTime data)
    {
        return _db.TabelasPreco
            .Where(t => t.DataInicioVigencia <= data.Date && t.DataFimVigencia >= data.Date)
            .OrderByDescending(t => t.DataInicioVigencia)
            .FirstOrDefault();
    }

    public void AtualizarRegistro(RegistroEstacionamento registro)
    {
        _db.Registros.Update(registro);
        _db.SaveChanges();
    }

    public bool ExisteSobreposicao(DateTime inicio, DateTime fim)
    {
        return _db.TabelasPreco.Any(t =>
            inicio <= t.DataFimVigencia && fim >= t.DataInicioVigencia);
    }

    public void AdicionarTabela(TabelaPreco tabela)
    {
        _db.TabelasPreco.Add(tabela);
        _db.SaveChanges();
    }

    public List<RegistroEstacionamento> ObterVeiculosEstacionados()
    {
        return _db.Registros
            .AsNoTracking()
            .Where(r => r.HorarioSaida == null)
            .OrderBy(r => r.HorarioEntrada)
            .ToList();
    }

    public List<RegistroEstacionamento> ObterHistorico(int limite)
    {
        return _db.Registros
            .AsNoTracking()
            .OrderByDescending(r => r.HorarioEntrada)
            .Take(limite)
            .ToList();
    }

    public List<TabelaPreco> ObterTabelasPreco()
    {
        return _db.TabelasPreco
            .AsNoTracking()
            .OrderByDescending(t => t.DataInicioVigencia)
            .ToList();
    }
}
