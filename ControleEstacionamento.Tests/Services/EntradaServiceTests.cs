using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services;
using Moq;
using Xunit;

namespace ControleEstacionamento.Tests.Services;

public class EntradaServiceTests
{
    private static readonly DateTimeOffset Agora = new(2026, 11, 20, 13, 0, 0, TimeSpan.Zero);

    [Fact]
    public void RegistrarEntrada_ComDadosValidos_DeveSalvarRegistroNormalizado()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ExisteEntradaAberta("ABC1D23")).Returns(false);
        dao.Setup(x => x.ExisteTabelaVigente(Agora.Date)).Returns(true);
        var service = new EntradaService(dao.Object, new FixedTimeProvider(Agora));

        service.RegistrarEntrada(new EntradaRequest { PlacaEntrada = "abc-1d23" });

        dao.Verify(x => x.AdicionarRegistro(It.Is<RegistroEstacionamento>(r =>
            r.Placa == "ABC1D23" && r.HorarioEntrada == Agora.UtcDateTime)), Times.Once);
    }

    [Fact]
    public void RegistrarEntrada_QuandoPlacaJaEstaAberta_DeveLancarExcecao()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ExisteEntradaAberta("ABC1234")).Returns(true);
        var service = new EntradaService(dao.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarEntrada(new EntradaRequest { PlacaEntrada = "ABC1234" }));

        dao.Verify(x => x.AdicionarRegistro(It.IsAny<RegistroEstacionamento>()), Times.Never);
    }

    [Fact]
    public void RegistrarEntrada_SemTarifaVigente_DeveLancarExcecao()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        dao.Setup(x => x.ExisteEntradaAberta("ABC1234")).Returns(false);
        dao.Setup(x => x.ExisteTabelaVigente(Agora.Date)).Returns(false);
        var service = new EntradaService(dao.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarEntrada(new EntradaRequest { PlacaEntrada = "ABC1234" }));

        dao.Verify(x => x.AdicionarRegistro(It.IsAny<RegistroEstacionamento>()), Times.Never);
    }

    [Fact]
    public void RegistrarEntrada_ComPlacaInvalida_DeveLancarExcecao()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        var service = new EntradaService(dao.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarEntrada(new EntradaRequest { PlacaEntrada = "ABC" }));

        dao.VerifyNoOtherCalls();
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now.ToUniversalTime();
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}
