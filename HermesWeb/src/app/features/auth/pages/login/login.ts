import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;
  mostrarSenha: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', Validators.required],
      lembrar: [false]
    });
  }

  toggleSenha(): void {
    this.mostrarSenha = !this.mostrarSenha;
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const { email, senha } = this.loginForm.value;

    this.authService.login({ email, senha }).subscribe({
      next: (response) => {
        const tipo = response.usuario.tipo;
        switch (tipo) {
          case 'Cliente':
            this.router.navigate(['/cliente']);
            break;
          case 'Transportador':
            this.router.navigate(['/transportador']);
            break;
          case 'Admin':
            this.router.navigate(['/admin']);
            break;
          default:
            this.router.navigate(['/']);
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Email ou senha inválidos';
        this.loading = false;
      }
    });
  }
}