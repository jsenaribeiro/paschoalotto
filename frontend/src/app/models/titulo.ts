import type { Parcela } from "./parcela";

export interface Titulo {
   id: number;
   numeroTitulo: string;
   nomeDevedor: string;
   cpfCnpj: string;
   dataVencimento: Date;
   dataEmissao: Date;
   parcelas: Parcela[];
   quantidadeParcelas: number;
   valorOriginal: number;
   diasEmAtraso: number;
   multa: number;
   jurosTotais: number;
   valorAtualizado: number;
   status: TituloStatus;
   total: { multa: 0, juros: 0, valor: 0 }
}

export enum TituloStatus {
   EM_ABERTO = 0,
   LIQUIDADO = 1,
   CANCELADO = 2,
}

export interface TituloDetalhado extends Titulo {
   enderecoDevedor?: string;
   telefoneDevedor?: string;
   emailDevedor?: string;
   observacoes?: string;
}
