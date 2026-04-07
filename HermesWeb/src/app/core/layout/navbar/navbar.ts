import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Route, Router } from '@angular/router';
import { Usuario } from '../../../models/usuario/usuario.model';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit {
  usuario: Usuario | null = null;
  dropdownOpen = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.usuario = user;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  getDashboardLink(): string {
    if (!this.usuario) return '/';
    switch (this.usuario.tipo) {
      case 'Cliente':
        return '/cliente';
      case 'Transportador':
        return '/transportador';
      case 'Admin':
        return '/admin/dashboard';  
      default: return '/';
    }
  }

  toggleDropdown(): void {
    this.dropdownOpen = !this.dropdownOpen;
  }
  
  get nomeCurto(): string {
    if (!this.usuario) return 'Visitante';
    if (this.usuario.nome) {
      const primeiroNome = this.usuario.nome.split(' ')[0];
      return primeiroNome;
    }
    if (this.usuario.email) {
      return this.usuario.email.split('@')[0];
    }
    return 'Usuário';
  }
}
