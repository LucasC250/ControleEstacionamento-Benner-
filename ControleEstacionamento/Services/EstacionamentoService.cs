using ControleEstacionamento.DAOs;
using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Models;

namespace ControleEstacionamento.Services;

public class EstacionamentoService
{
    private readonly EstacionamentoDAO _dao;

    public EstacionamentoService(EstacionamentoDAO dao)
    {
        _dao = dao;
    }

    public void RegistrarEntrada(EntradaRequest request)
    {
        var placa = request.ValidarPlaca();
        var horarioEntrada = DateTime.Now;

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

    public (decimal valor, DateTime entrada, DateTime saida, TimeSpan duracao) RegistrarSaida(SaidaRequest request)
    {
        var placa = request.ValidarPlaca();
        var registro = _dao.ObterEntradaAberta(placa);

        if (registro is null)
            throw new InvalidOperationException("Veículo não encontrado no estacionamento ou saída já registrada.");

        var horarioSaida = DateTime.Now;

        if (horarioSaida < registro.HorarioEntrada)
            throw new InvalidOperationException("O horário de saída não pode ser anterior ao horário de entrada.");

        var tabelaVigente = _dao.ObterTabelaPorData(registro.HorarioEntrada.Date);

        if (tabelaVigente is null)
            throw new InvalidOperationException("Não há tabela de preços vigente na data de entrada deste veículo.");

        var duracao = horarioSaida - registro.HorarioEntrada;
        var valor = CalcularValor(duracao, tabelaVigente);

        registro.HorarioSaida = horarioSaida;
        registro.ValorPago = valor;
        _dao.AtualizarRegistro(registro);

        return (valor, registro.HorarioEntrada, horarioSaida, duracao);
    }

    public void SalvarTabelaPreco(TabelaPrecoRequest request)
    {
        var novaTabela = request.ToEntity();

        var inicio = novaTabela.DataInicioVigencia;
        var fim = novaTabela.DataFimVigencia;

        if (_dao.ExisteSobreposicao(inicio, fim))
            throw new InvalidOperationException("Já existe uma tabela de preços com período de vigência sobreposto. Ajuste as datas.");

        _dao.AdicionarTabela(novaTabela);
    }

    public List<RegistroEstacionamento> ObterVeiculosEstacionados()
    {
        return _dao.ObterVeiculosEstacionados();
    }

    public List<RegistroEstacionamento> ObterHistorico(int limite = 200)
    {
        return _dao.ObterHistorico(limite);
    }

    public List<TabelaPreco> ObterTabelasPreco()
    {
        return _dao.ObterTabelasPreco();
    }

    public static decimal CalcularValor(TimeSpan duracao, TabelaPreco tabela)
    {
        if (duracao < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(duracao), "A duração não pode ser negativa.");

        var minutos = (decimal)duracao.TotalMinutes;

        if (minutos <= 30m)
            return Math.Round(tabela.ValorHoraInicial / 2m, 2, MidpointRounding.AwayFromZero);

        var adicionais = minutos <= 70m
            ? 0m
            : Math.Ceiling((minutos - 10m) / 60m) - 1m;

        return Math.Round(
            tabela.ValorHoraInicial + adicionais * tabela.ValorHoraAdicional,
            2,
            MidpointRounding.AwayFromZero);
    }
}
