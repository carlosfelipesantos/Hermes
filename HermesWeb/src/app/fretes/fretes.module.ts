import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { FretesRoutingModule } from './fretes-routing.module';
import { SolicitarFretes } from './pages/solicitar-fretes/solicitar-fretes';
import { LayoutModule } from '../core/layout/layout.module';

@NgModule({
  declarations: [
    SolicitarFretes
  ],
  imports: [
    CommonModule,
    FormsModule,
    FretesRoutingModule,
    LayoutModule
  ]
})
export class FretesModule {}