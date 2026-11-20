using ControleEstacionamento.Models;
using Xunit;

namespace ControleEstacionamento.Tests.Models;

public class RequestsTests
{
    [Theory]
    [InlineData("abc-1d23", "ABC1D23")]
    [InlineData("ABC 1D23", "ABC1D23")]
    [InlineData("abc1d23", "ABC1D23")]
    public void EntradaRequest_DeveNormalizarPlaca(string informada, string esperada)
    {
        var request = new EntradaRequest { PlacaEntrada = informada };

        Assert.Equal(esperada, request.ValidarPlaca());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("ABC123")]
    [InlineData("ABC123456")]
    public void SaidaRequest_ComPlacaInvalida_DeveLancarExcecao(string? placa)
    {
        var request = new SaidaRequest { PlacaSaida = placa };

        Assert.Throws<InvalidOperationException>(() => request.ValidarPlaca());
    }

    [Fact]
    public void TabelaPrecoRequest_Valida_DeveCriarEntidadeComDatasSemHorario()
    {
        var request = new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 1, 1, 15, 30, 0),
            DataFim = new DateTime(2026, 12, 31, 22, 0, 0),
            ValorHoraInicial = 10m,
            ValorHoraAdicional = 5m
        };

        var entidade = request.ToEntity();

        Assert.Equal(new DateTime(2026, 1, 1), entidade.DataInicioVigencia);
        Assert.Equal(new DateTime(2026, 12, 31), entidade.DataFimVigencia);
        Assert.Equal(10m, entidade.ValorHoraInicial);
        Assert.Equal(5m, entidade.ValorHoraAdicional);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(10, -1)]
    public void TabelaPrecoRequest_ComValoresInvalidos_DeveLancarExcecao(decimal inicial, decimal adicional)
    {
        var request = new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 1, 1),
            DataFim = new DateTime(2026, 12, 31),
            ValorHoraInicial = inicial,
            ValorHoraAdicional = adicional
        };

        Assert.Throws<InvalidOperationException>(() => request.ToEntity());
    }

    [Fact]
    public void TabelaPrecoRequest_ComFimAnteriorAoInicio_DeveLancarExcecao()
    {
        var request = new TabelaPrecoRequest
        {
            DataInicio = new DateTime(2026, 12, 31),
            DataFim = new DateTime(2026, 1, 1),
            ValorHoraInicial = 10m,
            ValorHoraAdicional = 5m
        };

        Assert.Throws<InvalidOperationException>(() => request.ToEntity());
    }
}
