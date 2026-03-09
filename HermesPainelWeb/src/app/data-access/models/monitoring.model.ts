export type MonitoringStatus = 'InProgress' | 'Stopped' | 'Canceled';

export interface MonitoringItem {
  id: string;

  origin: string;
  destination: string;

  status: MonitoringStatus;
  lastUpdateLabel: string; // "há 6 min", "há 3h"
  riskLabel: 'Ok' | 'Attention' | 'Critical';

  transporter: string;
  valueLabel: string;

  cargo: string;
}
