using ControleEstacionamento.EstacionamentoApp;
using ControleEstacionamento.Services;
using Xunit;

namespace ControleEstacionamento.Tests.Services;

public class CalculadoraPrecoServiceTests
{
    private readonly CalculadoraPrecoService _service = new();
    private readonly TabelaPreco _tabela = new()
    {
        ValorHoraInicial = 20m,
        ValorHoraAdicional = 10m
    };

    [Theory]
    [InlineData(0, 10)]
    [InlineData(30, 10)]
    public void CalcularValor_AteTrintaMinutos_CobraMetadeDaHoraInicial(int minutos, decimal esperado)
    {
        Assert.Equal(esperado, _service.CalcularValor(TimeSpan.FromMinutes(minutos), _tabela));
    }

    [Theory]
    [InlineData(31, 20)]
    [InlineData(70, 20)]
    [InlineData(71, 30)]
    [InlineData(130, 30)]
    [InlineData(131, 40)]
    public void CalcularValor_AposPeriodoDeTolerancia_CalculaHoraInicialEAdicionais(int minutos, decimal esperado)
    {
        Assert.Equal(esperado, _service.CalcularValor(TimeSpan.FromMinutes(minutos), _tabela));
    }

    [Theory]
    [InlineData(30, 1)]
    [InlineData(60, 2)]
    [InlineData(70, 2)]
    [InlineData(75, 3)]
    [InlineData(125, 3)]
    [InlineData(135, 4)]
    public void CalcularValor_DeveAtenderExemplosDoEnunciado(int minutos, decimal esperado)
    {
        var tabela = new TabelaPreco
        {
            ValorHoraInicial = 2m,
            ValorHoraAdicional = 1m
        };

        Assert.Equal(esperado, _service.CalcularValor(TimeSpan.FromMinutes(minutos), tabela));
    }

    [Fact]
    public void CalcularValor_ComDuracaoNegativa_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _service.CalcularValor(TimeSpan.FromMinutes(-1), _tabela));
    }
}
