export const formatCurrency = (value: number): string =>
   new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(value);

export const formatDate = (date: Date | string): string =>
   new Intl.DateTimeFormat('pt-BR').format(new Date(date));

export const formatNumber = (value: number): number => Math.round(value * 100) / 100;

export const format = (type: 'money' | 'date' | 'number', data: any) =>
   type === 'date' ? formatDate(data) :
      type === 'money' ? formatCurrency(data) :
         type === 'number' ? formatNumber(data) :
            data;