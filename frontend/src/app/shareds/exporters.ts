import { formatDate } from "./formats";

export function exportToCSV<T>(headers: string[], table: T[][], fileName: string): void {
   const csv = [headers.join(','), ...table.map(row => row.join(','))].join('\n');
   const blob = new Blob([csv], { type: 'text/csv' });
   const url = window.URL.createObjectURL(blob);
   const a = document.createElement('a');
   a.href = url;
   a.download = fileName;
   a.click();
}