import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReviewsService } from '../../data-access/services/review.service';
import { Review, ReviewsDashboardData, TransporterRatingSummary } from '../../data-access/models/review.model';

type PeriodFilter = '30d' | '7d' | 'month';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.css'],
})
export class ReviewsComponent {
  private service = inject(ReviewsService);

  loading = true;
  data: ReviewsDashboardData | null = null;

  period: PeriodFilter = '30d';
  query = '';

  drawerOpen = false;
  drawerLoading = false;
  selectedTransporter: TransporterRatingSummary | null = null;
  transporterReviews: Review[] = [];

  async ngOnInit() {
    this.data = await this.service.getDashboard();
    this.loading = false;
  }

  get filteredLatest(): Review[] {
    const q = this.query.trim().toLowerCase();
    if (!this.data) return [];

    return this.data.latestReviews.filter(r => {
      if (!q) return true;
      return (
        r.transporterName.toLowerCase().includes(q) ||
        r.customerName.toLowerCase().includes(q) ||
        r.freightId.toLowerCase().includes(q) ||
        r.comment.toLowerCase().includes(q)
      );
    });
  }

  stars(n: number) {
    return Array.from({ length: 5 }, (_, i) => i < n);
  }

  async openTransporter(t: TransporterRatingSummary) {
    this.drawerOpen = true;
    this.drawerLoading = true;
    this.selectedTransporter = t;
    this.transporterReviews = [];

    this.transporterReviews = await this.service.getTransporterReviews(t.transporterId);
    this.drawerLoading = false;
  }

  closeDrawer() {
    this.drawerOpen = false;
    this.selectedTransporter = null;
    this.transporterReviews = [];
  }
}
