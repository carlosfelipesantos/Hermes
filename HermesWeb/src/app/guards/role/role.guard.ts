import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { Router, CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (isPlatformServer(this.platformId)) {
      return true;
    }
    const expectedRoles = route.data['roles'] as Array<string>;
    const usuario = this.authService.getUsuario();
    if (usuario && expectedRoles.includes(usuario.tipo)) {
      return true;
    }
    this.router.navigate(['/']);
    return false;
  }
}