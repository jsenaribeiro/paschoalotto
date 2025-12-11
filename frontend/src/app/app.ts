import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { ToolbarModule } from 'primeng/toolbar';

@Component({
  standalone: true,
  selector: 'app-root',
  styleUrls: ['./app.css'],
  templateUrl: './app.html',
  imports: [CommonModule, RouterOutlet, RouterModule, ButtonModule, ToolbarModule],
})
export class AppComponent implements OnInit {
  public currentRoute = '';

  public ngOnInit() {
    console.log('Sistema de Cobrança iniciado');
  }

  public isActiveRoute(route: string): boolean {
    return this.currentRoute === route;
  }
}