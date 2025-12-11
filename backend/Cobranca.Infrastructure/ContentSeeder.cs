namespace Cobranca.Infrastructure;

using Cobranca.Domain;
using Cobranca.Domain.Clientes;
using Cobranca.Domain.Titulos;
using Microsoft.EntityFrameworkCore;

public static class ContentSeeder
{
   public static void SeedData(ModelBuilder mb)
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

      // Atribuir IDs (necesário para o seed)
      for (int i = 0; i < titulos.Count; i++)
      {
         typeof(Entity<int>).GetProperty("Id")!.SetValue(titulos[i], i + 1);
         typeof(Entity<int>).GetProperty("Id")!.SetValue(titulos[i].Devedor, i + 1);

         for (int j = 0; j < titulos[i].Parcelas.Count; j++)
         {
            var parcela = titulos[i].Parcelas.ElementAt(j);

            typeof(Entity<int>).GetProperty("Id")!.SetValue(parcela, i + j + 1);
            typeof(Entity<int>).GetProperty("TituloId")!.SetValue(parcela, titulos[i].Id);
         }
      }

      var clientes = titulos.Select(t => t.Devedor).ToList();
      var parcelas = titulos.SelectMany(t => t.Parcelas).ToList();

      mb.Entity<Titulo>().HasData(titulos);
      mb.Entity<Cliente>().HasData(clientes);
      mb.Entity<Parcela>().HasData(parcelas);
   }
}