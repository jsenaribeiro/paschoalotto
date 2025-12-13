namespace Cobranca.Infrastructure;

using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public static class DataBaseSeeder
{
    private record TituloSeedDto(string Numero, DateTime DataEmissao, decimal Valor,
        uint Parcelas, string Observacao, ClienteSeedDto Devedor, bool Cancelado);

    private record ClienteSeedDto(string Nome, string CpfCnpj, string Email, string Telefone, string Endereco);

    private static List<Titulo> CriarTitulos()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SeedData.json");
        var json = File.ReadAllText(path);
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var dtos = JsonSerializer.Deserialize<List<TituloSeedDto>>(json, opts) ?? [];
        var maps = (TituloSeedDto dto) =>
        {
            var devedor = new Cliente(dto.Devedor.Nome, dto.Devedor.CpfCnpj,
                dto.Devedor.Email, dto.Devedor.Telefone, dto.Devedor.Endereco);

            var titulo = new Titulo(dto.Numero, dto.DataEmissao,
                devedor, dto.Valor, dto.Parcelas, dto.Observacao);

            if (dto.Cancelado) titulo.Cancelar(DateTime.Now);

            return titulo;
        };

        uint parcelaId = 1;
        var titulos = dtos.Select(maps).ToList();

        for (int i = 0; i < titulos.Count; i++) // Atribuir IDs (necesário para o seed)
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
        var clientes = titulos.Select(x => x.Devedor).ToArray();
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
            .Select(x => new
            {
                x.Id,
                x.Telefone,
                x.CpfCnpj,
                x.Email,
                x.Endereco,
                x.Nome,
                Audit_CreatedAt = DateTime.Now
            }).ToList();

        var parcelasData = titulos.SelectMany(t => t.Parcelas)
            .Select(x => new
            {
                x.Id,
                x.DataPagamento,
                x.DataVencimento,
                x.Numero,
                x.Observacao,
                x.TituloId,
                x.Valor,
                x.ValorPago,
                Audit_CreatedAt = DateTime.Now
            }).ToList();

        var tituloData = titulos.Select(x => new
        {
            x.Id,
            x.DataEmissao,
            x.DevedorId,
            x.Numero,
            x.Observacao,
            x.Valor,
            x.Status,
            Audit_CreatedAt = DateTime.Now
        }).ToList();

        mb.Entity<Titulo>().HasData(tituloData);
        mb.Entity<Cliente>().HasData(clientesData);
        mb.Entity<Parcela>().HasData(parcelasData);
    }
}