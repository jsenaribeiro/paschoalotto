namespace Cobranca.Infrastructure;

using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;

public static class DataBaseSeeder
{
    private static List<Titulo> CriarTitulos()
    {
        var titulos = new List<Titulo>
        {
            new Titulo
            (
               "TIT-2023-001",
               new DateTime(2023, 3, 15),
               new Cliente("João Silva", "123.456.789-00", "joao.silva@email.com", "(11) 99999-9999", "Rua A, 123 - São Paulo/SP"),
               100,
               (uint)2,
               "Compra de equipamentos"
            ),

            new Titulo
            (
               "TIT-2023-002",
                new DateTime(2023, 4, 20),
                new Cliente("Maria Santos", "987.654.321-00", "maria.santos@email.com", "(21) 98888-8888", "Av. B, 456 - Rio de Janeiro/RJ"),
                50,
                (uint)1,
                "Serviço de consultoria"
            ),

            new Titulo
            (
                "TIT-2023-003",
                new DateTime(2023, 5, 10),
                new Cliente("Empresa XYZ Ltda", "12.345.678/0001-99", "contato@xyz.com", "(31) 97777-7777", "Rua C, 789 - Belo Horizonte/MG"),
                150,
                (uint)3,
                "Fornecimento de materiais"
            )
        };


        uint parcelaId = 1;

        // Atribuir IDs (necesário para o seed)
        for (int i = 0; i < titulos.Count; i++)
        {
            typeof(Titulo).GetProperty("Id")!.SetValue(titulos[i], (uint)(i + 1));
            typeof(Titulo).GetProperty("DevedorId")!.SetValue(titulos[i], (uint)(i + 1));
            typeof(Cliente).GetProperty("Id")!.SetValue(titulos[i].Devedor, (uint)(i + 1));

            for (int j = 0; j < titulos[i].Parcelas.Count; j++)
            {
                var parcela = titulos[i].Parcelas.ElementAt(j);

                typeof(Parcela).GetProperty("Id")!.SetValue(parcela, (uint)(parcelaId++));
                typeof(Parcela).GetProperty("TituloId")!.SetValue(parcela, titulos[i].Id);
            }
        }

        return titulos;
    }

    public static async Task SeedData(SqlDbContext db)
    {
        var titulos = CriarTitulos().ToArray();
        var clientes= titulos.Select(x => x.Devedor).ToArray();
        var parcelas = titulos.SelectMany(x => x.Parcelas).ToArray();

        await db.Titulos.AddRangeAsync(titulos);
        await db.Parcelas.AddRangeAsync(parcelas);
        await db.Clientes.AddRangeAsync(clientes);

        await db.SaveChangesAsync();
    }

    public static void SeedData(ModelBuilder mb)
    {
        var titulos = CriarTitulos();

        var clientesData = titulos.Select(t => t.Devedor)
            .Select(x => new { x.Id, x.Telefone, x.CpfCnpj, x.Email, 
                    x.Endereco, x.Nome, Audit_CreatedAt = DateTime.Now }).ToList();

        var parcelasData = titulos.SelectMany(t => t.Parcelas)
            .Select(x => new { x.Id, x.DataPagamento, x.DataVencimento, x.Numero, x.Observacao, 
                    x.TituloId, x.Valor, x.ValorPago, Audit_CreatedAt = DateTime.Now }).ToList();

        var tituloData = titulos.Select(x => new { x.Id, x.DataEmissao, x.DevedorId, 
            x.Numero, x.Observacao, x.Valor, x.Status, Audit_CreatedAt = DateTime.Now }).ToList();

        mb.Entity<Titulo>().HasData(tituloData);
        mb.Entity<Cliente>().HasData(clientesData);
        mb.Entity<Parcela>().HasData(parcelasData);
    }
}