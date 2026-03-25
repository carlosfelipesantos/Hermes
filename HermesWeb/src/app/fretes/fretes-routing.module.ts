import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SolicitarFretes } from './pages/solicitar-fretes/solicitar-fretes';

const routes: Routes = [
  { path: '', component: SolicitarFretes }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FretesRoutingModule {}