import { Routes } from '@angular/router';

export const routes: Routes = [{
   path: '',
   pathMatch: 'full',
   redirectTo: 'inicial'
},
{
   path: 'inicial',
   loadComponent: () => import('./routes/inicial/inicial.component')
      .then(m => m.InicialComponent)
},
{
   path: 'titulo',
   loadComponent: () => import('./routes/titulos/titulos.component')
      .then(m => m.TituloListComponent)
},
{
   path: '**',
   redirectTo: 'inicial'
}];