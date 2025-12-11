import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { CobrancaResumo, DashboardMetrics, Titulo } from '../../models';

@Component({
   standalone: true,
   selector: 'app-inicial',
   styleUrls: ['./inicial.component.css'],
   templateUrl: './inicial.component.html',
   imports: [CommonModule, RouterModule, ButtonModule, CardModule, ToolbarModule, TableModule, FormsModule]
})
export class InicialComponent implements OnInit {
   public metrics = signal<DashboardMetrics | null>(null);
   public resumo = signal<CobrancaResumo | null>(null);
   public titulos = signal<Titulo[]>([]);
   public loading = signal(true);
   public chartData = {
      labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
      datasets: [{
         label: 'Títulos em Atraso',
         data: [12, 19, 8, 15, 22, 18],
         borderColor: '#0079B8',
         backgroundColor: 'rgba(0, 121, 184, 0.1)',
         tension: 0.4
      }]
   };

   public ngOnInit(): void { }
}