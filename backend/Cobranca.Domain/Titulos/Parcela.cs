using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cobranca.Domain.Titulos;

public class Parcela : Entity<uint>
{
    const decimal PERCENTUAL_MULTA = 0.02m; // 2%

    const decimal TAXA_JUROS_MENSAL = 0.01m; // 1% ao mês

    public uint Numero { get; private set; }

    /// <summary>
    /// Valor original
    /// </summary>
    public decimal Valor { get; private set; }

    public DateTime DataVencimento { get; private set; }

    public DateTime? DataPagamento { get; private set; }

    public ParcelaStatus Status => DataPagamento.HasValue ? ParcelaStatus.Paga
        : DataVencimento.Date < DateTime.Today ? ParcelaStatus.Vencida
        : ParcelaStatus.EmAberto;

    public Titulo? Titulo { get; protected set; }

    public uint TituloId { get; protected set; }

    public string? Observacao { get; set; }

    public decimal ValorPago { get; private set; }

    public Parcela(uint numero, decimal valor, DateTime dataVencimento)
    {
        if (numero == 0) throw new ArgumentException("Número da parcela deve maior que zero");
        if (valor == 0) throw new ArgumentException("Valor da parcela deve maior que zero");

        Valor = valor;
        Numero = numero;
        DataVencimento = dataVencimento;
    }

    public void Pagar(decimal valor)
    {
        if (Status == ParcelaStatus.Paga) throw new InvalidOperationException("Parcela já está paga");
        if (valor < Total) throw new ArgumentException("Valor pago é menor que o total da parcela");

        ValorPago = valor;
        DataPagamento = DateTime.Now;
    }

    public void Ajustar(decimal valor)
    {
        if (Status == ParcelaStatus.Paga) throw new InvalidOperationException("Parcela já está paga");

        Valor = valor;
    }

    /// <summary>
    /// Valor calculado
    /// </summary>
    public decimal Total => Valor + Multa + Juros;

    public decimal Multa => DiasDeAtraso <= 0 ? 0 : Valor * PERCENTUAL_MULTA;

    public decimal Juros
    {
        get
        {
            if (DiasDeAtraso == 0) return 0;

            var mes = DataVencimento.Month;
            var ano = DataVencimento.Year;
            var diasDoMes = DateTime.DaysInMonth(ano, mes);
            var jurosDiario = TAXA_JUROS_MENSAL / diasDoMes;// TODO: corrigido para a quantidade de dias do mes real, ao inves de /30 (doc)

            return jurosDiario * DiasDeAtraso;
        }
    }

    public uint DiasDeAtraso => DataVencimento.Date >= DateTime.Today ? 0
        : (uint)(DateTime.Today - DataVencimento.Date).TotalDays;
}