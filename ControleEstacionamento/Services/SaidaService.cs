using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services.Interfaces;

namespace ControleEstacionamento.Services;

public class SaidaService : ISaidaService
{
    private readonly IEstacionamentoDAO _dao;
    private readonly ICalculadoraPrecoService _calculadoraPreco;
    private readonly TimeProvider _timeProvider;

    public SaidaService(
        IEstacionamentoDAO dao,
        ICalculadoraPrecoService calculadoraPreco,
        TimeProvider timeProvider)
    {
        _dao = dao;
        _calculadoraPreco = calculadoraPreco;
        _timeProvider = timeProvider;
    }

    public ResultadoSaida RegistrarSaida(SaidaRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var placa = request.ValidarPlaca();
        var registro = _dao.ObterEntradaAberta(placa);

        if (registro is null)
            throw new InvalidOperationException("Veículo não encontrado no estacionamento ou saída já registrada.");

        var horarioSaida = _timeProvider.GetLocalNow().DateTime;

        if (horarioSaida < registro.HorarioEntrada)
            throw new InvalidOperationException("O horário de saída não pode ser anterior ao horário de entrada.");

        var tabelaVigente = _dao.ObterTabelaPorData(registro.HorarioEntrada.Date);

        if (tabelaVigente is null)
            throw new InvalidOperationException("Não há tabela de preços vigente na data de entrada deste veículo.");

        var duracao = horarioSaida - registro.HorarioEntrada;
        var valor = _calculadoraPreco.CalcularValor(duracao, tabelaVigente);

        registro.HorarioSaida = horarioSaida;
        registro.ValorPago = valor;
        _dao.AtualizarRegistro(registro);

        return new ResultadoSaida(valor, registro.HorarioEntrada, horarioSaida, duracao);
    }
}
