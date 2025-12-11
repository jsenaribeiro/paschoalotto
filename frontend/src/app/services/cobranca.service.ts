import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Titulo, TituloDetalhado, CobrancaResumo, DashboardMetrics } from '../models';
import { TituloStatus } from '../models';
import { catchPipe, formatNumber } from '../shareds';
import { getFakeTitulos } from './titulos.fake';

interface CalculoCobranca { multa: number; juros: number; valor: number; }

@Injectable({ providedIn: 'root' })
export class CobrancaService {
   private http = inject(HttpClient);
   private apiUrl = 'http://localhost:5000/api';

   public getTitulos = (): Observable<Titulo[]> =>
      this.http.get<Titulo[]>(`${this.apiUrl}/titulos`)
         .pipe(catchPipe<Titulo[]>(this.getTitulos.name, []));

   public getTitulosFake = (): Observable<Titulo[]> => new Observable((observer: any) => {
      setTimeout(() => { observer.next(getFakeTitulos()); observer.complete(); }, 500);
   });

   public getTitulosEmAtraso = (): Observable<Titulo[]> =>
      this.http.get<Titulo[]>(`${this.apiUrl}/titulos/atraso`)
         .pipe(catchPipe<Titulo[]>(this.getTitulosEmAtraso.name, []));

   public getTituloById = (id: number): Observable<TituloDetalhado> =>
      this.http.get<TituloDetalhado>(`${this.apiUrl}/titulos/${id}`)
         .pipe(catchPipe<TituloDetalhado>(this.getTituloById.name, []));
   public getResumoCobranca = (): Observable<CobrancaResumo> =>
      this.http.get<CobrancaResumo>(`${this.apiUrl}/titulos/atraso/resumo`)
         .pipe(catchPipe<CobrancaResumo>(this.getResumoCobranca.name, []));

   public getDashboardMetrics = (): Observable<DashboardMetrics> =>
      this.http.get<DashboardMetrics>(`${this.apiUrl}/dashboard/metrics`)
         .pipe(catchPipe<DashboardMetrics>(this.getDashboardMetrics.name, []));

   public getStatusBadge(status: TituloStatus): { class: string, text: string } {
      const statusMap: Record<TituloStatus, { class: string, text: string }> = {
         'EM_ABERTO': { class: 'label label-info', text: 'Em Aberto' },
         'PAGO': { class: 'label label-success', text: 'Pago' },
         'CANCELADO': { class: 'label label-danger', text: 'Cancelado' },
         'NEGOCIADO': { class: 'label label-warning', text: 'Negociado' }
      };

      return statusMap[status] || { class: 'label', text: status };
   }

   public getAtrasoBadge(dias: number): { class: string, text: string } {
      if (dias === 0) return { class: 'label label-info', text: 'Em dia' };
      if (dias <= 30) return { class: 'label label-warning', text: `${dias} dias` };
      if (dias <= 90) return { class: 'label label-danger', text: `${dias} dias` };
      return { class: 'label label-purple', text: `${dias} dias` };
   }

   public calcularCobranca(valorOriginal: number, diasAtraso: number): CalculoCobranca {
      const multa = formatNumber(valorOriginal * 0.02);
      const juros = formatNumber(valorOriginal * (0.01 / 30) * diasAtraso);
      const valor = formatNumber(valorOriginal + multa + juros);

      return { multa, juros, valor }
   }
}

