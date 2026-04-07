import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { ClientePage } from './home/cliente-page/cliente-page';
import { TransportadorPage } from './home/transportador-page/transportador-page';
import { AuthGuard } from './guards/auth/auth.guard';
import { RoleGuard } from './guards/role/role.guard';

const routes: Routes = [
  { path: '', component: Home },
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'fretes',
    loadChildren: () => import('./fretes/fretes.module').then(m => m.FretesModule)
  },

  {
    path: 'cliente',
    component: ClientePage,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Cliente'] }
  },
  {
    path: 'transportador',
    component: TransportadorPage,
    canActivate: [AuthGuard, RoleGuard],
    data: { roles: ['Transportador'] }
  },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}