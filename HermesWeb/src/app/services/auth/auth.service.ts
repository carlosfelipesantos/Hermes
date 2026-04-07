import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Usuario, LoginRequest, LoginResponse } from '../../models/usuario/usuario.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Auth`;
  private tokenKey = 'token';
  private usuarioKey = 'usuario';
  private currentUserSubject = new BehaviorSubject<Usuario | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    if (isPlatformBrowser(this.platformId)) {
      this.loadStoredUser();
    }
  }

  private loadStoredUser(): void {
    const token = this.getToken();
    const usuario = this.getUsuario();
    if (token && usuario) {
      this.currentUserSubject.next(usuario);
    }
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(tap(response => {
        if (isPlatformBrowser(this.platformId)) {
          this.setToken(response.token);
          this.setUsuario(response.usuario);
          this.currentUserSubject.next(response.usuario);
        }
      }));
  }

  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.tokenKey);
      localStorage.removeItem(this.usuarioKey);
      this.currentUserSubject.next(null);
    }
  }

  getToken(): string | null {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  private setToken(token: string): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.tokenKey, token);
    }
  }

  getUsuario(): Usuario | null {
    if (isPlatformBrowser(this.platformId)) {
      const usuario = localStorage.getItem(this.usuarioKey);
      return usuario ? JSON.parse(usuario) : null;
    }
    return null;
  }

  private setUsuario(usuario: Usuario): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.usuarioKey, JSON.stringify(usuario));
    }
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  hasRole(role: string): boolean {
    const usuario = this.getUsuario();
    return usuario?.tipo === role;
  }
}