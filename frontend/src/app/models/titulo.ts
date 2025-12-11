import { Parcela } from "./parcela";

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

export type TituloStatus = 'EM_ABERTO' | 'PAGO' | 'CANCELADO' | 'NEGOCIADO';

export interface TituloDetalhado extends Titulo {
   parcelas: Parcela[];
   enderecoDevedor?: string;
   telefoneDevedor?: string;
   emailDevedor?: string;
   observacoes?: string;
}



