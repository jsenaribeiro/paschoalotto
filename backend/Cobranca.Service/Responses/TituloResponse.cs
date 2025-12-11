namespace Cobranca.Service.Responses;

using Cobranca.Domain.Titulos;

public record TituloResponse(
    string NumeroTitulo,
    string NomeDevedor,
    uint QuantidadeParcelas,
    decimal ValorOriginal,
    uint DiasEmAtraso,
    decimal Multa,
    decimal JurosTotais,
    decimal ValorAtualizado
)
{
    public TituloResponse() : this(
        NumeroTitulo: string.Empty,
        NomeDevedor: string.Empty,
        QuantidadeParcelas: 0,
        ValorOriginal: 0m,
        DiasEmAtraso: 0,
        Multa: 0m,
        JurosTotais: 0m,
        ValorAtualizado: 0m
    )
    { }

    public TituloResponse(Titulo titulo) : this(
        NumeroTitulo: titulo.Numero,
        NomeDevedor: titulo.Devedor.Nome,
        QuantidadeParcelas: (uint)titulo.Parcelas.Count,
        ValorOriginal: titulo.Valor,
        DiasEmAtraso: (uint)titulo.Parcelas.Sum(p => p.DiasDeAtraso),
        Multa: titulo.Total.Multa,
        JurosTotais: titulo.Total.Juros,
        ValorAtualizado: titulo.Total.Valor
    )
    { }
}


