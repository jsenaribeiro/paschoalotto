import { CommonModule } from "@angular/common";
import {
   Component,
   computed,
   type OnInit,
   signal,
   ViewChild,
} from "@angular/core";
import { FormsModule } from "@angular/forms";
import { type Router, RouterModule } from "@angular/router";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { type Table, TableModule } from "primeng/table";
import { ToolbarModule } from "primeng/toolbar";
import { debounceTime, distinctUntilChanged, Subject } from "rxjs";
import type { CobrancaResumo, Titulo } from "../../models";
// biome-ignore lint/style/useImportType: nao é usado apenas como tipo
import { CobrancaService } from "../../services";
import { format } from "../../shareds";

@Component({
   standalone: true,
   selector: "app-titulo",
   styleUrls: ["./titulos.component.css"],
   templateUrl: "./titulos.component.html",
   imports: [
      CommonModule,
      RouterModule,
      ButtonModule,
      CardModule,
      ToolbarModule,
      TableModule,
      FormsModule,
   ],
})
export class TituloListComponent implements OnInit {
   @ViewChild("datagridRef") datagridRef: Table | undefined;

   public titulos = signal<Titulo[]>([]);
   public selectedTitulos = signal<Titulo[]>([]);
   public resumo = signal<CobrancaResumo | null>(null);
   public loading = signal(true);
   public searchTerm = "";
   public statusFilter = "";
   public searchSubject = new Subject<string>();
   public pageSize = 10;
   public currentPage = 1;
   public total = 0;

   public format = format;

   public totalSelecionado = computed(() =>
      this.selectedTitulos().reduce((sum, t) => sum + t.valorAtualizado, 0),
   );

   constructor(public cs: CobrancaService) {
      this.searchSubject
         .pipe(debounceTime(300), distinctUntilChanged())
         .subscribe(() => this.loadTitulos());
   }

   public ngOnInit(): void {
      this.loadTitulos();
   }

   public atrasoDescricao(dias: number): string {
      if (dias === 0) return "Sem atraso";
      if (dias === 1) return "1 dia";
      return `${dias} dias`;
   }

   public loadTitulos(): void {
      this.loading.set(true);

      this.cs.getTitulos().subscribe({
         next: (titulos) => this.titulos.set(titulos),
         error: (error) => console.error("Erro ao carregar títulos:", error),
      });

      this.cs.getTitulos().subscribe({
         next: (titulos) => console.log(titulos),
         error: (error) => console.error("Erro ao carregar títulos:", error),
      });
   }

   public getProgressWidth = (value: number, max: number = 100): string =>
      `${Math.min((value / max) * 100, 100)}%`;
}
