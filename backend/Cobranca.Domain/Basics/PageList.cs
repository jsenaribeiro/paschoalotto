namespace Cobranca.Domain;

public record PageList(int Total);

public record PageList<E>(E[] Items, int Total) : PageList(Total);