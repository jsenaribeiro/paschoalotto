using Cobranca.Domain.Titulos;

namespace Cobranca.Domain.Clientes;

public class Cliente : Entity<uint>
{
    protected Cliente() { }

    public Cliente(string nome, string cpfCnpj)
    {

        Nome = nome;
        CpfCnpj = cpfCnpj;

        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome do cliente é obrigatório");

        if (string.IsNullOrWhiteSpace(CpfCnpj))
            throw new ArgumentException("CPF/CNPJ é obrigatório");
    }

    public Cliente(string nome, string cpfCnpj, string email, string telefone) : this(nome, cpfCnpj)
    {
        Email = email;
        Telefone = telefone;
    }

    public Cliente(string nome, string cpfCnpj, string email, string telefone, string endereco) : this(nome, cpfCnpj, email, endereco)
    {
        Endereco = endereco;
    }

    public string Nome { get; private set; } = string.Empty;
    
    public string CpfCnpj { get; private set; } = string.Empty;
    
    public string? Email { get; private set; }

    public string? Telefone { get; private set; }

    public string? Endereco { get; private set; }


    private readonly List<Titulo> _titulos = new();

    public IReadOnlyCollection<Titulo> Titulos => _titulos.AsReadOnly();

    public static Cliente Empty => new Cliente();
}
