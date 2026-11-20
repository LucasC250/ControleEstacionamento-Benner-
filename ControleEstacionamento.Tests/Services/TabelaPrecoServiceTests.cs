using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services;
using Moq;
using Xunit;

namespace ControleEstacionamento.Tests.Services;

public class TabelaPrecoServiceTests
{
    [Fact]
    public void SalvarTabelaPreco_ComPeriodoSemSobreposicao_DeveSalvar()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ExisteSobreposicao(new DateTime(2026, 1, 1), new DateTime(2026, 12, 31))).Returns(false);
        var service = new TabelaPrecoService(dao.Object);
        var request = new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 1, 1),
            DataFim = new DateTime(2026, 12, 31),
            ValorHoraInicial = 15m,
            ValorHoraAdicional = 5m
        };

        service.SalvarTabelaPreco(request);

        dao.Verify(x => x.AdicionarTabela(It.Is<TabelaPreco>(t =>
            t.DataInicioVigencia == new DateTime(2026, 1, 1) &&
            t.DataFimVigencia == new DateTime(2026, 12, 31) &&
            t.ValorHoraInicial == 15m &&
            t.ValorHoraAdicional == 5m)), Times.Once);
    }

    [Fact]
    public void SalvarTabelaPreco_ComPeriodoSobreposto_DeveLancarExcecao()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ExisteSobreposicao(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);
        var service = new TabelaPrecoService(dao.Object);

        Assert.Throws<InvalidOperationException>(() => service.SalvarTabelaPreco(new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 5, 1),
            DataFim = new DateTime(2026, 6, 30),
            ValorHoraInicial = 15m,
            ValorHoraAdicional = 5m
        }));

        dao.Verify(x => x.AdicionarTabela(It.IsAny<TabelaPreco>()), Times.Never);
    }

    [Fact]
    public void SalvarTabelaPreco_ComValoresInvalidos_DeveLancarExcecaoAntesDeConsultarBanco()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        var service = new TabelaPrecoService(dao.Object);

        Assert.Throws<InvalidOperationException>(() => service.SalvarTabelaPreco(new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 5, 1),
            DataFim = new DateTime(2026, 6, 30),
            ValorHoraInicial = 0,
            ValorHoraAdicional = 5m
        }));

        dao.VerifyNoOtherCalls();
    }

    [Fact]
    public void ObterTabelasPreco_DeveDelegarConsultaAoDao()
    {
        var tabelas = new List<TabelaPreco> { new() { Id = 1 } };
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ObterTabelasPreco()).Returns(tabelas);
        var service = new TabelaPrecoService(dao.Object);

        var resultado = service.ObterTabelasPreco();

        Assert.Same(tabelas, resultado);
    }
}
