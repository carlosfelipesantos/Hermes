import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css'
})

export class Login {
  email = '';
  senha = '';
  lembrar = false;
  mostrarSenha = false;

  mensagem = '';
  tipoMensagem: 'success' | 'error' | '' = '';

  toggleSenha(): void {
    this.mostrarSenha = !this.mostrarSenha;
  }

  entrar(): void {
    this.tipoMensagem = 'success';
    this.mensagem = 'Visual do login pronto. A autenticação será ligada à API depois.';
  }
}