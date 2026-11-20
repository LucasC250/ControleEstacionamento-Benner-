using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Services.Interfaces;

namespace ControleEstacionamento.Services;

public class TabelaPrecoService : ITabelaPrecoService
{
    private readonly IEstacionamentoDAO _dao;

    public TabelaPrecoService(IEstacionamentoDAO dao)
    {
        _dao = dao;
    }

    public void SalvarTabelaPreco(TabelaPrecoRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        TabelaPreco novaTabela = request.ToEntity();

        if (_dao.ExisteSobreposicao(novaTabela.DataInicioVigencia, novaTabela.DataFimVigencia))
            throw new InvalidOperationException("Já existe uma tabela de preços com período de vigência sobreposto. Ajuste as datas.");

        _dao.AdicionarTabela(novaTabela);
    }

    public List<TabelaPreco> ObterTabelasPreco()
    {
        return _dao.ObterTabelasPreco();
    }
}
