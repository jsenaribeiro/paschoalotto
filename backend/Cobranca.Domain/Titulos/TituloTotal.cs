using Cobranca.Domain.Clientes;

namespace Cobranca.Domain.Titulos;

public record TituloTotal(decimal Valor, decimal Pago, decimal Multa, decimal Juros, decimal Divida)
{
    public static TituloTotal From(Parcela[] parcelas)
    {
        var valor = parcelas.Sum(p => p.Total);
        var pago = parcelas.Where(p => p.DataPagamento != null).Sum(p => p.Valor);
        var multa = parcelas.Sum(p => p.Multa);
        var juros = parcelas.Sum(p => p.Juros);
        var divida = valor - pago;

        return new TituloTotal(valor, pago, multa, juros, divida);
    }
}