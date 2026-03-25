import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { App } from './app';
import { Home } from './home/home';
import { FormsModule } from '@angular/forms';
import { LayoutModule } from './core/layout/layout.module';
import { ClientePage } from './home/cliente-page/cliente-page';
import { TransportadorPage } from './home/transportador-page/transportador-page';

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
  ],
  bootstrap: [App]
})
export class AppModule { }
