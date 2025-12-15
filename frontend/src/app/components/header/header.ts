import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, ButtonModule, ToolbarModule],
  template: `
    <p-toolbar class="border-none border-bottom-1 surface-border border-round">
      <div class="p-toolbar-group-start flex gap-2 p-4">
        <i class=" pi pi-wallet text-2xl text-primary"></i>
        <span class="text-xl text-900 px-1">Sistema de Cobrança | <b>Paschoalotto</b></span>
      </div>
      <div class="p-toolbar-group-end gap-2">
        <p-button label="Apresentação" icon="pi pi-home" class="px-1" 
          routerLink="/inicial" routerLinkActive="route-active"
          [routerLinkActiveOptions]="{exact: true}" />

        <p-button label="Demonstração" icon="pi pi-list" 
          routerLink="/titulo" routerLinkActive="route-active" />
      </div>
    </p-toolbar>
  `
})
export class HeaderComponent {}
