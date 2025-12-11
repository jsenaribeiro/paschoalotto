namespace Cobranca.Service.Responses;

public class ParcelaResponse
{
    public int Numero { get; init; }
    public decimal Valor { get; init; }
    public required DateTime DataVencimento { get; init; }
}