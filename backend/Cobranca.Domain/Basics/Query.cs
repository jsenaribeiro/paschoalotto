namespace Cobranca.Domain;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Order { ASC = 0, DESC }

[Flags]
public enum QueryFlags
{
   Default = 0,
   NoTracked = 1 << 0,
   ShowDeleteds = 1 << 1
}

public record PageList(int Total);

public record PageList<E>(E[] Items, int Total) : PageList(Total);