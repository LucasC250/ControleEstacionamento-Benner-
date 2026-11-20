using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Models;

public abstract record PlacaRequest
{
    protected abstract string? PlacaInformada { get; }

    public string ValidarPlaca()
    {
        if (string.IsNullOrWhiteSpace(PlacaInformada))
            throw new InvalidOperationException("Informe a placa do veículo.");

        var placa = new string(PlacaInformada
            .Trim()
            .Where(char.IsLetterOrDigit)
            .ToArray())
            .ToUpperInvariant();

        if (placa.Length != 7)
            throw new InvalidOperationException("Informe uma placa válida com 7 caracteres.");

        return placa;
    }
}

public sealed record EntradaRequest : PlacaRequest
{
    public string? PlacaEntrada { get; set; }

    protected override string? PlacaInformada => PlacaEntrada;
}

public sealed record SaidaRequest : PlacaRequest
{
    public string? PlacaSaida { get; set; }

    protected override string? PlacaInformada => PlacaSaida;
}

public sealed record TabelaPrecoRequest
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal ValorHoraInicial { get; set; }
    public decimal ValorHoraAdicional { get; set; }

    public TabelaPreco ToEntity()
    {
        if (DataInicio == default || DataFim == default)
            throw new InvalidOperationException("Informe as datas de início e fim da vigência.");

        if (ValorHoraInicial <= 0 || ValorHoraAdicional < 0)
            throw new InvalidOperationException("A hora inicial deve ser maior que zero e a hora adicional não pode ser negativa.");

        if (DataFim.Date < DataInicio.Date)
            throw new InvalidOperationException("A data final não pode ser anterior à data inicial.");

        return new TabelaPreco
        {
            DataInicioVigencia = DataInicio.Date,
            DataFimVigencia = DataFim.Date,
            ValorHoraInicial = ValorHoraInicial,
            ValorHoraAdicional = ValorHoraAdicional
        };
    }
}

public sealed record ResultadoSaida(
    decimal Valor,
    DateTime Entrada,
    DateTime Saida,
    TimeSpan Duracao);
