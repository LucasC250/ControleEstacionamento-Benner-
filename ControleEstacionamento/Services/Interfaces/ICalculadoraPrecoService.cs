using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Services.Interfaces;

public interface ICalculadoraPrecoService
{
    decimal CalcularValor(TimeSpan duracao, TabelaPreco tabela);
}
