export interface Review {
  id: string;
  freightId: string;

  transporterName: string;
  transporterId: string;

  customerName: string;

  rating: number;
  comment: string;

  createdAt: string; 
  dateLabel: string; 
}

export interface TransporterRatingSummary {
  transporterId: string;
  transporterName: string;

  average: number;
  totalReviews: number;

  lastComment?: string;
}

export interface ReviewsDashboardData {
  averageRating: number;
  totalReviews: number;
  ratedTransporters: number;

  topTransporters: TransporterRatingSummary[];
  latestReviews: Review[];
}
