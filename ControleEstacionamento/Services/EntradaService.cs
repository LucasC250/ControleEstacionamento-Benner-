using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services.Interfaces;

namespace ControleEstacionamento.Services;

public class EntradaService : IEntradaService
{
    private readonly IEstacionamentoDAO _dao;
    private readonly TimeProvider _timeProvider;

    public EntradaService(IEstacionamentoDAO dao, TimeProvider timeProvider)
    {
        _dao = dao;
        _timeProvider = timeProvider;
    }

    public void RegistrarEntrada(EntradaRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var placa = request.ValidarPlaca();
        var horarioEntrada = _timeProvider.GetLocalNow().DateTime;

        if (_dao.ExisteEntradaAberta(placa))
            throw new InvalidOperationException("Este veículo já possui uma entrada em aberto.");

        if (!_dao.ExisteTabelaVigente(horarioEntrada.Date))
            throw new InvalidOperationException("Cadastre uma tabela de preços vigente para hoje antes de registrar entradas.");

        _dao.AdicionarRegistro(new RegistroEstacionamento
        {
            Placa = placa,
            HorarioEntrada = horarioEntrada
        });
    }
}
