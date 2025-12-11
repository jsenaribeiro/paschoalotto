import { Component, OnInit, ViewChild, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';
import { CobrancaService } from '../../services/cobranca.service';
import { exportToCSV, format, formatCurrency, formatDate } from '../../shareds';
import { CobrancaResumo, Titulo } from '../../models';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';

@Component({
   standalone: true,
   selector: 'app-titulo',
   styleUrls: ['./titulos.component.css'],
   templateUrl: './titulos.component.html',
   imports: [CommonModule, RouterModule, ButtonModule, CardModule, ToolbarModule, TableModule, FormsModule]
})
export class TituloListComponent implements OnInit {
   @ViewChild('datagridRef') datagridRef: any;

   public titulos = signal<Titulo[]>([]);
   public selectedTitulos = signal<Titulo[]>([]);
   public resumo = signal<CobrancaResumo | null>(null);
   public loading = signal(true);
   public searchTerm = '';
   public statusFilter = '';
   public searchSubject = new Subject<string>();
   public pageSize = 10;
   public currentPage = 1;
   public total = 0;

   public format = format;

   public totalSelecionado = computed(() => this.selectedTitulos()
      .reduce((sum, t) => sum + t.valorAtualizado, 0));

   constructor(public cobrancaService: CobrancaService, private router: Router) {
      this.searchSubject.pipe(debounceTime(300), distinctUntilChanged()).subscribe(() => this.loadTitulos());
   }

   public ngOnInit(): void {
      this.loadTitulos();
   }

   public atrasoDescricao(dias: number): string {
      if (dias === 0) return 'Sem atraso';
      if (dias === 1) return '1 dia';
      return `${dias} dias`;
   }


   public loadTitulos(): void {
      this.loading.set(true);

      this.cobrancaService.getTitulosFake().subscribe({
         next: (titulos) => this.titulos.set(titulos),
         error: (error) => console.error('Erro ao carregar títulos:', error)
      });

      this.cobrancaService.getResumoCobranca().subscribe({
         next: (resumo) => {
            this.resumo.set(resumo);
            this.loading.set(false);
         },
         error: (error) => {
            console.error('Erro ao carregar resumo:', error);
            this.loading.set(false);
         }
      });
   }

   public getProgressWidth = (value: number, max: number = 100): string =>
      `${Math.min((value / max) * 100, 100)}%`;

}