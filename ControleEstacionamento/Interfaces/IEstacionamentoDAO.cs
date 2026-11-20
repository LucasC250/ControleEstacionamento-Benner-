using ControleEstacionamento.EstacionamentoApp;

namespace ControleEstacionamento.Interfaces;

public interface IEstacionamentoDAO
{
    bool ExisteEntradaAberta(string placa);
    bool ExisteTabelaVigente(DateTime data);
    void AdicionarRegistro(RegistroEstacionamento registro);
    RegistroEstacionamento? ObterEntradaAberta(string placa);
    TabelaPreco? ObterTabelaPorData(DateTime data);
    void AtualizarRegistro(RegistroEstacionamento registro);
    bool ExisteSobreposicao(DateTime inicio, DateTime fim);
    void AdicionarTabela(TabelaPreco tabela);
    List<RegistroEstacionamento> ObterVeiculosEstacionados();
    List<RegistroEstacionamento> ObterHistorico(int limite);
    List<TabelaPreco> ObterTabelasPreco();
}
