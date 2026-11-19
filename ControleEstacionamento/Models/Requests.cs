using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Models;

public abstract class PlacaRequest
{
    protected abstract string? PlacaInformada { get; }

    public string ValidarPlaca()
    {
        if (string.IsNullOrWhiteSpace(PlacaInformada))
            throw new InvalidOperationException("Informe a placa do veículo.");

        var placa = PlacaInformada.Trim().ToUpperInvariant();
        placa = new string(placa.Where(char.IsLetterOrDigit).ToArray());

        if (placa.Length != 7)
            throw new InvalidOperationException("Informe uma placa válida com 7 caracteres.");

        return placa;
    }
}

public class EntradaRequest : PlacaRequest
{
    public string? PlacaEntrada { get; set; }

    protected override string? PlacaInformada => PlacaEntrada;
}

public class SaidaRequest : PlacaRequest
{
    public string? PlacaSaida { get; set; }

    protected override string? PlacaInformada => PlacaSaida;
}

public class TabelaPrecoRequest
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

        var tabela = new TabelaPreco();
        tabela.DataInicioVigencia = DataInicio.Date;
        tabela.DataFimVigencia = DataFim.Date;
        tabela.ValorHoraInicial = ValorHoraInicial;
        tabela.ValorHoraAdicional = ValorHoraAdicional;

        return tabela;
    }
}
