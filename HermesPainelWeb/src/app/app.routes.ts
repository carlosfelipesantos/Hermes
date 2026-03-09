import { Routes } from '@angular/router';
import { ShellComponent } from './core/shell/shell.component';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'users', loadComponent: () => import('./features/users/users.component').then(m => m.UsersComponent) },
      { path: 'freights', loadComponent: () => import('./features/freights/freights.component').then(m => m.FreightsComponent) },
      { path: 'create-freights', loadComponent: () => import('./features/freights/create-freights/create-freights.component').then(m => m.CreateFreightsComponent) },
      { path: 'monitoring', loadComponent: () => import('./features/monitoring/monitoring.component').then(m => m.MonitoringComponent) },
      { path: 'reviews', loadComponent: () => import('./features/reviews/reviews.component').then(m => m.ReviewsComponent) },
      { path: 'vehicles', loadComponent: () => import('./features/vehicles/vehicles.component').then(m => m.VehiclesComponent) },
      { path: 'notifications', loadComponent: () => import('./features/notifications/notifications.component').then(m => m.NotificationsComponent) },
      { path: 'settings', loadComponent: () => import('./features/settings/settings.component').then(m => m.SettingsComponent) },
      { path: 'account', loadComponent: () => import('./features/settings/account/account.component').then(m => m.AccountComponent) },
    ],
  },
  { path: '**', redirectTo: 'dashboard' },
];
