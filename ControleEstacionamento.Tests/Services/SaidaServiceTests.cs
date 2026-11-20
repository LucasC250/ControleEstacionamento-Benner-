using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Interfaces;
using ControleEstacionamento.Models;
using ControleEstacionamento.Services;
using ControleEstacionamento.Services.Interfaces;
using Moq;
using Xunit;

namespace ControleEstacionamento.Tests.Services;

public class SaidaServiceTests
{
    private static readonly DateTimeOffset Agora = new(2026, 11, 20, 13, 0, 0, TimeSpan.Zero);

    [Fact]
    public void RegistrarSaida_ComEntradaETarifaDaDataDeEntrada_DeveCalcularEAtualizarRegistro()
    {
        var entrada = Agora.UtcDateTime.AddMinutes(-90);
        var registro = new RegistroEstacionamento
        {
            Placa = "ABC1234",
            HorarioEntrada = entrada
        };
        var tabela = new TabelaPreco { ValorHoraInicial = 15m, ValorHoraAdicional = 5m };
        var dao = new Mock<IEstacionamentoDAO>();
        var calculadora = new Mock<ICalculadoraPrecoService>();

        dao.Setup(x => x.ObterEntradaAberta("ABC1234")).Returns(registro);
        dao.Setup(x => x.ObterTabelaPorData(entrada.Date)).Returns(tabela);
        calculadora.Setup(x => x.CalcularValor(TimeSpan.FromMinutes(90), tabela)).Returns(20m);

        var service = new SaidaService(dao.Object, calculadora.Object, new FixedTimeProvider(Agora));
        var resultado = service.RegistrarSaida(new SaidaRequest { PlacaSaida = "ABC1234" });

        Assert.Equal(20m, resultado.Valor);
        Assert.Equal(entrada, resultado.Entrada);
        Assert.Equal(Agora.UtcDateTime, resultado.Saida);
        Assert.Equal(TimeSpan.FromMinutes(90), resultado.Duracao);
        Assert.Equal(Agora.UtcDateTime, registro.HorarioSaida);
        Assert.Equal(20m, registro.ValorPago);
        dao.Verify(x => x.AtualizarRegistro(registro), Times.Once);
        calculadora.Verify(x => x.CalcularValor(TimeSpan.FromMinutes(90), tabela), Times.Once);
    }

    [Fact]
    public void RegistrarSaida_QuandoNaoExisteEntradaAberta_DeveLancarExcecao()
    {
        var dao = new Mock<IEstacionamentoDAO>();
        var calculadora = new Mock<ICalculadoraPrecoService>();
        dao.Setup(x => x.ObterEntradaAberta("ABC1234")).Returns((RegistroEstacionamento?)null);
        var service = new SaidaService(dao.Object, calculadora.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarSaida(new SaidaRequest { PlacaSaida = "ABC1234" }));

        dao.Verify(x => x.AtualizarRegistro(It.IsAny<RegistroEstacionamento>()), Times.Never);
    }

    [Fact]
    public void RegistrarSaida_SemTarifaDaDataDeEntrada_DeveLancarExcecao()
    {
        var registro = new RegistroEstacionamento
        {
            Placa = "ABC1234",
            HorarioEntrada = Agora.UtcDateTime.AddDays(-1)
        };
        var dao = new Mock<IEstacionamentoDAO>();
        var calculadora = new Mock<ICalculadoraPrecoService>();
        dao.Setup(x => x.ObterEntradaAberta("ABC1234")).Returns(registro);
        dao.Setup(x => x.ObterTabelaPorData(registro.HorarioEntrada.Date)).Returns((TabelaPreco?)null);
        var service = new SaidaService(dao.Object, calculadora.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarSaida(new SaidaRequest { PlacaSaida = "ABC1234" }));

        dao.Verify(x => x.AtualizarRegistro(It.IsAny<RegistroEstacionamento>()), Times.Never);
        calculadora.VerifyNoOtherCalls();
    }

    [Fact]
    public void RegistrarSaida_QuandoHorarioAtualAnteriorAEntrada_DeveLancarExcecao()
    {
        var registro = new RegistroEstacionamento
        {
            Placa = "ABC1234",
            HorarioEntrada = Agora.UtcDateTime.AddMinutes(5)
        };
        var dao = new Mock<IEstacionamentoDAO>();
        var calculadora = new Mock<ICalculadoraPrecoService>();
        dao.Setup(x => x.ObterEntradaAberta("ABC1234")).Returns(registro);
        var service = new SaidaService(dao.Object, calculadora.Object, new FixedTimeProvider(Agora));

        Assert.Throws<InvalidOperationException>(() =>
            service.RegistrarSaida(new SaidaRequest { PlacaSaida = "ABC1234" }));

        dao.Verify(x => x.AtualizarRegistro(It.IsAny<RegistroEstacionamento>()), Times.Never);
    }

    private sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => now.ToUniversalTime();
        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}
