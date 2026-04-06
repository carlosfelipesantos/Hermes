import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { App } from './app';
import { Home } from './home/home';
import { FormsModule } from '@angular/forms';
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

@NgModule({
  declarations: [
    App,
    Home,
    ClientePage,
    TransportadorPage  
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    LayoutModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideClientHydration(withEventReplay()),
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
