using ControleEstacionamento.Models;
using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Services.Interfaces;

public interface ITabelaPrecoService
{
    void SalvarTabelaPreco(TabelaPrecoRequest request);
    List<TabelaPreco> ObterTabelasPreco();
}
