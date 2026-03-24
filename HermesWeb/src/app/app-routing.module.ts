import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { App } from './app';
import { Home } from './home/home';

const routes: Routes = [
    { path: '', component: Home }, 
    {
      path: 'auth',
      loadChildren: () =>
      import('./features/auth/auth.module').then(m => m.AuthModule)
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}