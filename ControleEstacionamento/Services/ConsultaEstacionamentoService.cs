using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Services.Interfaces;

namespace ControleEstacionamento.Services;

public class ConsultaEstacionamentoService : IConsultaEstacionamentoService
{
    private readonly IEstacionamentoDAO _dao;

    public ConsultaEstacionamentoService(IEstacionamentoDAO dao)
    {
        _dao = dao;
    }

    public List<RegistroEstacionamento> ObterVeiculosEstacionados()
    {
        return _dao.ObterVeiculosEstacionados();
    }

    public List<RegistroEstacionamento> ObterHistorico(int limite = 200)
    {
        if (limite <= 0)
            throw new ArgumentOutOfRangeException(nameof(limite), "O limite deve ser maior que zero.");

        return _dao.ObterHistorico(limite);
    }
}
