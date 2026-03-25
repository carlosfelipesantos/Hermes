import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { App } from './app';
import { Home } from './home/home';
import { ClientePage } from './home/cliente-page/cliente-page';
import { TransportadorPage } from './home/transportador-page/transportador-page';

const routes: Routes = [
    { path: '', component: Home }, 
    {
      path: 'auth',
      loadChildren: () =>
      import('./features/auth/auth.module').then(m => m.AuthModule)
    },
    {
      path: 'fretes',
      loadChildren: () =>
      import('./fretes/fretes.module').then(m => m.FretesModule)
    },

    {
      path: 'home/cliente', component: ClientePage, 
    },
    {
      path: 'home/transportador', component: TransportadorPage, 
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}