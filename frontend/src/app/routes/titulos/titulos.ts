import {
   Component,
   computed,
   type OnInit,
   signal,
   ViewChild,
} from "@angular/core";
import { type Table } from "primeng/table";
import type { Titulo } from "../../models";
import { CobrancaService } from "../../services";
import { format } from "../../shareds";
import { pageModules } from '../../shareds';

@Component({ standalone: true,
             imports: pageModules,
             selector: "app-titulo",
             styleUrls: ["./titulos.css"],
             templateUrl: "./titulos.html" })
export class TituloListComponent implements OnInit {
   @ViewChild("datagridRef") datagridRef: Table | undefined;

   public titulos = signal<Titulo[]>([]);
   public loading = signal(true);
   public procura = signal("");

   constructor(public cs: CobrancaService) {}

   public filtrados = computed(() => {
      const term = this.procura().toLowerCase();
      const allTitulos = this.titulos();

      if (!term) return allTitulos;

      return allTitulos.filter(t => 
         t.nomeDevedor.toLowerCase().includes(term) || 
         t.numeroTitulo.toLowerCase().includes(term)
      );
   });

   public totalSelecionado = computed(() =>
      this.filtrados().reduce((sum, t) => sum + t.valorAtualizado, 0),
   );

   public format = format;

   public ngOnInit = (): void => this.loadTitulos();

   public atraso = (dias: number): string => 
      dias === 0 ? "Sem atraso" : `${dias} dia` + (dias == 1 ? "" : "s");

   public loadTitulos(): void {
      this.loading.set(true);

      this.cs.getTitulos().subscribe({
         next: (data) => {
            this.titulos.set(data);
            this.loading.set(false);
         },
         error: (error) => {
            console.error("Erro ao carregar títulos:", error);
            this.loading.set(false);
         },
      });
   }

   public onSearch = (term: string): void => this.procura.set(term);

   public getProgressWidth = (value: number, max: number = 100): string =>
      `${Math.min((value / max) * 100, 100)}%`;
}
