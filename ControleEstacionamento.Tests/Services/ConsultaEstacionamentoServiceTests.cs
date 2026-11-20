using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Services;
using Moq;
using Xunit;

namespace ControleEstacionamento.Tests.Services;

public class ConsultaEstacionamentoServiceTests
{
    [Fact]
    public void ObterVeiculosEstacionados_DeveRetornarResultadoDoDao()
    {
        var registros = new List<RegistroEstacionamento> { new() { Placa = "ABC1234" } };
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ObterVeiculosEstacionados()).Returns(registros);
        var service = new ConsultaEstacionamentoService(dao.Object);

        var resultado = service.ObterVeiculosEstacionados();

        Assert.Same(registros, resultado);
    }

    [Fact]
    public void ObterHistorico_DeveRepassarLimiteAoDao()
    {
        var registros = new List<RegistroEstacionamento> { new() { Placa = "ABC1234" } };
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ObterHistorico(25)).Returns(registros);
        var service = new ConsultaEstacionamentoService(dao.Object);

        var resultado = service.ObterHistorico(25);

        Assert.Same(registros, resultado);
        dao.Verify(x => x.ObterHistorico(25), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ObterHistorico_ComLimiteInvalido_DeveLancarExcecao(int limite)
    {
        var dao = new Mock<IEstacionamentoDAO>();
        var service = new ConsultaEstacionamentoService(dao.Object);

        Assert.Throws<ArgumentOutOfRangeException>(() => service.ObterHistorico(limite));
        dao.VerifyNoOtherCalls();
    }
}
