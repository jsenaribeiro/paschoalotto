import { Titulo } from "../models";

export function getFakeTitulos(): Titulo[] {
   const today = new Date();
   const baseTitulos: Partial<Titulo>[] = [
      {
         id: 1,
         numeroTitulo: '123456',
         nomeDevedor: 'João Silva',
         cpfCnpj: '123.456.789-00',
         dataVencimento: new Date('2025-01-15'),
         dataEmissao: new Date('2024-12-15'),
         quantidadeParcelas: 1,
         valorOriginal: 1500.00,
         status: 'EM_ABERTO'
      },
      {
         id: 2,
         numeroTitulo: '789012',
         nomeDevedor: 'Maria Santos',
         cpfCnpj: '987.654.321-00',
         dataVencimento: new Date('2024-11-20'),
         dataEmissao: new Date('2024-10-20'),
         quantidadeParcelas: 2,
         valorOriginal: 2500.50,
         status: 'EM_ABERTO'
      },
      {
         id: 3,
         numeroTitulo: '345678',
         nomeDevedor: 'Empresa XYZ Ltda',
         cpfCnpj: '12.345.678/0001-90',
         dataVencimento: new Date('2024-12-01'),
         dataEmissao: new Date('2024-11-01'),
         quantidadeParcelas: 1,
         valorOriginal: 5000.00,
         status: 'NEGOCIADO'
      },
      {
         id: 4,
         numeroTitulo: '901234',
         nomeDevedor: 'Pedro Oliveira',
         cpfCnpj: '456.789.123-00',
         dataVencimento: new Date('2024-10-15'),
         dataEmissao: new Date('2024-09-15'),
         quantidadeParcelas: 3,
         valorOriginal: 750.00,
         status: 'CANCELADO'
      },
      {
         id: 5,
         numeroTitulo: '567890',
         nomeDevedor: 'Ana Costa',
         cpfCnpj: '789.123.456-00',
         dataVencimento: new Date('2025-02-10'),
         dataEmissao: new Date('2025-01-10'),
         quantidadeParcelas: 1,
         valorOriginal: 1200.00,
         status: 'PAGO'
      }
   ];

   return baseTitulos.map(t => {
      const vencimento = t.dataVencimento!;
      const diffTime = Math.max(0, today.getTime() - vencimento.getTime());
      const diasAtraso = Math.floor(diffTime / (1000 * 60 * 60 * 24));

      let multa = 0;
      let juros = 0;

      if (diasAtraso > 0 && t.status === 'EM_ABERTO') {
         multa = t.valorOriginal! * 0.02;
         juros = t.valorOriginal! * (0.01 / 30) * diasAtraso;
      }

      return {
         ...t,
         diasEmAtraso: diasAtraso,
         multa: multa,
         jurosTotais: juros,
         valorAtualizado: t.valorOriginal! + multa + juros
      } as Titulo;
   });
}