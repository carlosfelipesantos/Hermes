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
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loading = true;
      this.errorMessage = '';
      
      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          // Redirecionar baseado no tipo de usuário
          const tipo = response.usuario.tipo;
          if (tipo === 'Cliente') {
            this.router.navigate(['/cliente/dashboard']);
          } else if (tipo === 'Transportador') {
            this.router.navigate(['/transportador/dashboard']);
          } else if (tipo === 'Admin') {
            this.router.navigate(['/admin/dashboard']);
          }
        },
        error: (error) => {
          this.errorMessage = 'Email ou senha inválidos';
          this.loading = false;
        }
      });
    }
  }
}