import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AuthRoutingModule } from './auth-routing.module';
import { Login } from './pages/login/login';
import { Cadastro } from './pages/cadastro/cadastro';

@NgModule({
  declarations: [
    Login,
    Cadastro
  ],
  imports: [
    CommonModule,
    FormsModule,
    AuthRoutingModule
  ]
})
export class AuthModule {}