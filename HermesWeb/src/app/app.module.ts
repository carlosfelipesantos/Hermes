import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { App } from './app';
import { Home } from './home/home';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from './core/layout/layout.module';
import { ClientePage } from './home/cliente-page/cliente-page';
import { TransportadorPage } from './home/transportador-page/transportador-page';

import { AuthService } from './services/auth/auth.service';
import { UsuarioService } from './services/usuario/usuario.service';
import { FreteService } from './services/frete/frete.service';
import { VeiculoService } from './services/veiculo/veiculo.service';
import { DisponibilidadeService } from './services/disponibilidade/disponibilidade.service';
import { AdminService } from './services/admin/admin.service';
import { NotificacaoService } from './services/notificacoes/notificacoes.service';

import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withFetch,
  withInterceptorsFromDi
} from '@angular/common/http';

import { AuthInterceptor } from './interceptors/auth/auth.interceptor';
import { Perfil} from './perfil/pages/perfil/perfil';

@NgModule({
  declarations: [
    App,
    Home,
    ClientePage,
    TransportadorPage,
    Perfil
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    LayoutModule,
    ReactiveFormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),

    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },

    provideHttpClient(
      withInterceptorsFromDi(),
      withFetch()
    ),

    AuthService,
    UsuarioService,
    FreteService,
    VeiculoService,
    DisponibilidadeService,
    AdminService,
    NotificacaoService
  ],
  bootstrap: [App]
})
export class AppModule { }