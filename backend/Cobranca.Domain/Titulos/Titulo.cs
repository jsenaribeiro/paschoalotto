using Cobranca.Domain.Clientes;

namespace Cobranca.Domain.Titulos;

public class Titulo : Entity<uint>
{
    protected Titulo() { }

    public static Titulo Empty => new Titulo();

    public Titulo(string numero, Cliente devedor, DateTime dataEmissao, decimal valor, uint parcelas)
    {
        if (string.IsNullOrWhiteSpace(numero)) throw new ArgumentException("Número do título é obrigatório");
        if (valor < 1) throw new ArgumentException("O valor do título deve ser maior do que zero");
        if (parcelas < 1) throw new ArgumentException("Título deve ter pelo menos uma parcela");
        if (devedor == null) throw new ArgumentException("Devedor é obrigatório");

        Numero = numero;
        Devedor = devedor;
        DataEmissao = dataEmissao;
        Status = TituloStatus.EmAberto;
        Valor = valor;

        Parcelar(parcelas); ;
    }

    public Titulo(string numero, DateTime dataEmissao, Cliente devedor, decimal valor, uint parcelas, string observacao)
        : this(numero, devedor, dataEmissao, valor, parcelas) { this.Observacao = observacao; }

    public string Numero { get; private set; } = string.Empty;

    public Cliente Devedor { get; private set; } = null!;

    public uint DevedorId { get; private set; }

    public decimal Valor { get; private set; }

    public string? Observacao { get; private set; }

    public TituloStatus Status { get; private set; }

    public DateTime DataEmissao { get; set; }

    public TituloTotal Total => TituloTotal.From(_parcelas.ToArray());


    private readonly List<Parcela> _parcelas = new();

    public bool EmAtraso => Status == TituloStatus.EmAberto
        && this.Parcelas.Where(p => p.DataVencimento < DateTime.Today)
                        .Any(p => p.DataPagamento == null);

    public IReadOnlyCollection<Parcela> Parcelas => _parcelas.AsReadOnly();

    public void Parcelar(uint quantidade)
    {
        if (quantidade < 1) throw new ArgumentException("Quantidade de parcelas deve ser maior que zero");

        var valorParcela = Math.Round(Valor / quantidade, 2);

        _parcelas.Clear();

        for (int i = 1; i <= quantidade; i++)
            _parcelas.Add(new Parcela((uint)i, valorParcela, DataEmissao.AddMonths(i)));

        var valorTotalParcelas = _parcelas.Sum(p => p.Valor);
        var diferenca = Valor - valorTotalParcelas;

        if (diferenca > 0)
        {
            var ultimaParcela = _parcelas.Last();
            var valorDaUltimaParcela = ultimaParcela.Valor;
            var novoValorAjustado = valorDaUltimaParcela + diferenca;

            ultimaParcela.Ajustar(novoValorAjustado);
        }
    }

    public void Cancelar(DateTime dataPagamento) => Status = TituloStatus.Cancelado;
}