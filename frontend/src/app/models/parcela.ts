export interface Parcela {
   id: number;
   numero: number;
   valor: number;
   dataVencimento: Date;
   dataPagamento?: Date;
   status: ParcelaStatus;
}

export type ParcelaStatus = 'EM_ABERTO' | 'PAGA' | 'VENCIDA';