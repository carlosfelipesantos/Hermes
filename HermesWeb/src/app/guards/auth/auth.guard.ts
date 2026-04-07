import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  canActivate(): boolean {
    if (isPlatformServer(this.platformId)) {
      return true;
    }
    const token = this.authService.getToken();
    if (token) {
      return true;
    }
    this.router.navigate(['/auth/login']);
    return false;
  }
}