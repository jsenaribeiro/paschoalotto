import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoadingService } from './loading.service';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule, ProgressSpinnerModule],
  template: `
    @if (loadingService.isLoading()) {
      <div class="loading-overlay fade-in">
        <p-progressSpinner 
          styleClass="w-4rem h-4rem" 
          strokeWidth="4" 
          fill="var(--surface-ground)" 
          animationDuration=".5s">
        </p-progressSpinner>
      </div>
    }
  `,
  styles: [`
    .loading-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.4); /* Fundo escurecido */
      backdrop-filter: blur(4px); /* Efeito de desfoque */
      z-index: 9999;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .fade-in {
      animation: fadeIn 0.2s ease-in-out;
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }
  `]
})
export class LoadingComponent {
  loadingService = inject(LoadingService);
}
