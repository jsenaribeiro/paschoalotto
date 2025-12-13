import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import type { Observable } from "rxjs";
import type {
   CobrancaResumo,
   DashboardMetrics,
   Titulo,
   TituloDetalhado,
   TituloStatus,
} from "../models";
import { catchPipe, formatNumber } from "../shareds";

interface CalculoCobranca {
   multa: number;
   juros: number;
   valor: number;
}

@Injectable({ providedIn: "root" })
export class CobrancaService {
   private http = inject(HttpClient);
   private apiUrl = "http://localhost:5000/api";
   private subscription = null as any;

   public getTitulos = (): Observable<Titulo[]> =>
      this.http
         .get<Titulo[]>(`${this.apiUrl}/titulos`)
         .pipe(catchPipe<Titulo[]>(this.getTitulos.name, []));

   public getTitulosEmAtraso = (): Observable<Titulo[]> =>
      this.http
         .get<Titulo[]>(`${this.apiUrl}/titulos/atraso`)
         .pipe(catchPipe<Titulo[]>(this.getTitulosEmAtraso.name, []));

   public getTituloById = (id: number): Observable<TituloDetalhado> =>
      this.http
         .get<TituloDetalhado>(`${this.apiUrl}/titulos/${id}`)
         .pipe(catchPipe<TituloDetalhado>(this.getTituloById.name, []));

   public getStatusBadge(status: TituloStatus): { class: string; text: string } {
      const statusArray = [
         { class: "label label-info", text: "Em Aberto" },
         { class: "label label-success", text: "Pago" },
         { class: "label label-danger", text: "Cancelado" },
         { class: "label label-warning", text: "Negociado" },
      ];

      return statusArray[status] || { class: "label", text: status };
   }

   public getAtrasoBadge(dias: number): { class: string; text: string } {
      if (dias === 0) return { class: "label label-info", text: "Em dia" };
      if (dias <= 30)
         return { class: "label label-warning", text: `${dias} dias` };
      if (dias <= 90)
         return { class: "label label-danger", text: `${dias} dias` };
      return { class: "label label-purple", text: `${dias} dias` };
   }

   public calcularCobranca(
      valorOriginal: number,
      diasAtraso: number,
   ): CalculoCobranca {
      const multa = formatNumber(valorOriginal * 0.02);
      const juros = formatNumber(valorOriginal * (0.01 / 30) * diasAtraso);
      const valor = formatNumber(valorOriginal + multa + juros);

      return { multa, juros, valor };
   }
}
