using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Services.Interfaces;

namespace ControleEstacionamento.Services;

public class CalculadoraPrecoService : ICalculadoraPrecoService
{
    public decimal CalcularValor(TimeSpan duracao, TabelaPreco tabela)
    {
        ArgumentNullException.ThrowIfNull(tabela);

        if (duracao < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(duracao), "A duração não pode ser negativa.");

        var minutos = (decimal)duracao.TotalMinutes;

        if (minutos <= 30m)
            return Math.Round(tabela.ValorHoraInicial / 2m, 2, MidpointRounding.AwayFromZero);

        var adicionais = minutos <= 70m
            ? 0m
            : Math.Ceiling((minutos - 70m) / 60m);

        return Math.Round(
            tabela.ValorHoraInicial + adicionais * tabela.ValorHoraAdicional,
            2,
            MidpointRounding.AwayFromZero);
    }
}
