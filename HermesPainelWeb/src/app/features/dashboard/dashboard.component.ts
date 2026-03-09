import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { DashboardService } from '../../data-access/services/dashboard.service';
import { DashboardData } from '../../data-access/models/dashboard.model';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  private service = inject(DashboardService);

  loading = true;
  data: DashboardData | null = null;

  async ngOnInit() {
    this.data = await this.service.get();
    this.loading = false;
  }

  toneClass(tone: 'neutral' | 'warn' | 'ok' | 'bad') {
    return {
      neutral: tone === 'neutral',
      warn: tone === 'warn',
      ok: tone === 'ok',
      bad: tone === 'bad',
    };
  }

  alertClass(type: 'NewFreight' | 'Operational' | 'System') {
    return {
      ok: type === 'NewFreight',
      warn: type === 'Operational',
      sys: type === 'System',
    };
  }

  ratingStars(avg: number) {
    // só visual simples
    const full = Math.floor(avg);
    const half = avg - full >= 0.5;
    return { full, half };
  }
}
