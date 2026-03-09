export interface DashboardKpis {
  requestedFreights: number;
  inProgressFreights: number;
  doneFreights: number;
  usersCount: number;
  activeTransporters: number;
  averageRating: number; 
}

export type AlertType = 'NewFreight' | 'Operational' | 'System';

export interface DashboardAlert {
  id: string;
  type: AlertType;
  title: string;
  description: string;
  timeLabel: string; 
}

export interface DashboardLatestFreight {
  id: string;
  statusLabel: string; 
  statusTone: 'neutral' | 'warn' | 'ok' | 'bad';
  origin: string;
  destination: string;
  valueLabel: string; 
  transporterLabel: string; 
}

export interface DashboardData {
  kpis: DashboardKpis;
  latestFreights: DashboardLatestFreight[];
  alerts: DashboardAlert[];
}
