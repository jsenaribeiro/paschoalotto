namespace Cobranca.Test.Titulos;

using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Shouldly;

public class TituloTest
{
    private Cliente NovoCliente() => new("12345678900", "Fulano", "teste@email.com", "(85)1234.56789");

    [Fact]
    public void SeNumeroVazio_DeveLancarExcecao()
    {
        var cliente = NovoCliente();

        var ex = Assert.Throws<ArgumentException>(() => new Titulo("", cliente, DateTime.Today, 100, 2));

        ex.Message.ShouldContain("Número do título");
    }

    [Fact]
    public void SeValorDoTituloMenorQue1_DeveLancarExcecao()
    {
        var cliente = NovoCliente();

        var ex = Assert.Throws<ArgumentException>(() => new Titulo("1", cliente, DateTime.Today, 0, 2));

        ex.Message.ShouldContain("O valor do título deve ser maior do que zero");
    }

    [Fact]
    public void SeParcelasMenorQueUm_DeveLancarExcecao()
    {
        var cliente = NovoCliente();

        var ex = Assert.Throws<ArgumentException>(() => new Titulo("123", cliente, DateTime.Today, 100, 0));

        ex.Message.ShouldContain("pelo menos uma parcela");
    }

    [Fact]
    public void SeDevedorNulo_DeveLancarExcecao()
    {
        var ex = Assert.Throws<ArgumentException>(() => new Titulo("123", null, DateTime.Today, 100,2));

        ex.Message.ShouldContain("Devedor é obrigatório");
    }

    [Fact]
    public void Parcelar_DeveCriarParcelasComQuantidadeCorreta()
    {
        var cliente = NovoCliente();
        var titulo = new Titulo("123", cliente, DateTime.Today, 100, 3);

        titulo.Parcelas.Count.ShouldBe(3);
    }

    [Fact]
    public void Parcelar_DeveLancarExcecao_SeQuantidadeZero()
    {
        var cliente = NovoCliente();
        var titulo = new Titulo("123", cliente, DateTime.Today, 100, 2);

        Assert.Throws<ArgumentException>(() => titulo.Parcelar(0));
    }

    [Fact]
    public void Cancelar_DeveAlterarStatusParaCancelado()
    {
        var cliente = NovoCliente();
        var titulo = new Titulo("123", cliente, DateTime.Today, 100, 2);

        titulo.Cancelar(DateTime.Today);

        titulo.Status.ShouldBe(StatusTitulo.Cancelado);
    }

    [Fact]
    public void EmAtraso_DeveSerTrue_SeAlgumaParcelaVencidaENaoPaga()
    {
        var cliente = NovoCliente();
        var titulo = new Titulo("123", cliente, DateTime.Today.AddMonths(-3), 100, 2);

        var parcelaVencida = titulo.Parcelas.First();

        // Força a data de vencimento da primeira parcela para o passado
        typeof(Parcela).GetProperty("DataVencimento")!
            .SetValue(parcelaVencida, DateTime.Today.AddDays(-10));

        titulo.EmAtraso.ShouldBeTrue();
    }

    [Fact]
    public void EmAtraso_DeveSerFalse_SeTodasParcelasPagas()
    {
        var cliente = NovoCliente();
        var titulo = new Titulo("123", cliente, DateTime.Today.AddMonths(-3), 100, 2);

        foreach (var parcela in titulo.Parcelas)
            parcela.Pagar(parcela.Total);

        titulo.EmAtraso.ShouldBeFalse();
    }
}