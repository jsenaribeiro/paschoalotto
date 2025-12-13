
namespace Cobranca.Test.Titulos;

using Xunit;
using System;
using Cobranca.Domain.Titulos;

public class ParcelaTest
{
    [Fact]
    public void Construtor_DeveLancarExcecao_SeNumeroZero()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Parcela(0, 100, DateTime.Today.AddDays(1))
        );

        Assert.Contains("Número da parcela", ex.Message);
    }

    [Fact]
    public void Construtor_DeveLancarExcecao_SeValorZero()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            new Parcela(1, 0, DateTime.Today.AddDays(1))
        );

        Assert.Contains("Valor da parcela", ex.Message);
    }

    [Fact]
    public void Status_DeveSerEmAberto_SeNaoVencidaENaoPaga()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(1));

        Assert.Equal(ParcelaStatus.EmAberto, parcela.Status);
    }

    [Fact]
    public void Status_DeveSerVencida_SeVencidaENaoPaga()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-1));

        Assert.Equal(ParcelaStatus.Vencida, parcela.Status);
    }

    [Fact]
    public void Status_DeveSerPaga_AposPagamento()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-1));

        parcela.Pagar(parcela.Total);

        Assert.Equal(ParcelaStatus.Paga, parcela.Status);
    }

    [Fact]
    public void Pagar_DeveLancarExcecao_SeValorMenorQueTotal()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-5));
        Assert.Throws<ArgumentException>(() => parcela.Pagar(parcela.Total - 1));
    }

    [Fact]
    public void Pagar_DeveLancarExcecao_SeJaPaga()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-5));

        parcela.Pagar(parcela.Total);

        Assert.Throws<InvalidOperationException>(() => parcela.Pagar(parcela.Total));
    }

    [Fact]
    public void Ajustar_DeveAlterarValor_SeNaoPaga()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(1));

        parcela.Ajustar(200);

        Assert.Equal(200, parcela.Valor);
    }

    [Fact]
    public void Ajustar_DeveLancarExcecao_SePaga()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-1));

        parcela.Pagar(parcela.Total);

        Assert.Throws<InvalidOperationException>(() => parcela.Ajustar(200));
    }

    [Fact]
    public void Multa_DeveSerZero_SeNaoVencida()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(1));

        Assert.Equal(0, parcela.Multa);
    }

    [Fact]
    public void Multa_DeveSerDoisPorCento_SeVencida()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(-2));

        Assert.Equal(2, parcela.Multa); // 2% de 100
    }

    [Fact]
    public void Juros_DeveSerZero_SeNaoVencida()
    {
        var parcela = new Parcela(1, 100, DateTime.Today.AddDays(1));

        Assert.Equal(0, parcela.Juros);
    }

    [Fact]
    public void Juros_DeveSerCalculado_SeVencida()
    {
        var vencimento = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - 2);
        var parcela = new Parcela(1, 100, vencimento);
        var diasAtraso = parcela.DiasDeAtraso;
        var diasNoMes = DateTime.DaysInMonth(vencimento.Year, vencimento.Month);
        var jurosEsperado = (0.01m / diasNoMes) * diasAtraso;

        Assert.Equal(jurosEsperado, parcela.Juros, 4);
    }

    [Fact]
    public void Total_DeveSerValorMaisMultaMaisJuros()
    {
        var vencimento = DateTime.Today.AddDays(-3);
        var parcela = new Parcela(1, 100, vencimento);
        var esperado = parcela.Valor + parcela.Multa + parcela.Juros;

        Assert.Equal(esperado, parcela.Total, 4);
    }
}