import type { Parcela } from "./parcela";

export interface Titulo {
   id: number;
   numeroTitulo: string;
   nomeDevedor: string;
   cpfCnpj: string;
   dataVencimento: Date;
   dataEmissao: Date;
   quantidadeParcelas: number;
   valorOriginal: number;
   diasEmAtraso: number;
   multa: number;
   jurosTotais: number;
   valorAtualizado: number;
   status: TituloStatus;
}

export enum TituloStatus {
   EM_ABERTO = 0,
   LIQUIDADO = 1,
   CANCELADO = 2,
}

export interface TituloDetalhado extends Titulo {
   parcelas: Parcela[];
   enderecoDevedor?: string;
   telefoneDevedor?: string;
   emailDevedor?: string;
   observacoes?: string;
}
