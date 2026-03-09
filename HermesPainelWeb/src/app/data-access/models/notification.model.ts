export type NotificationType = 'NewFreight' | 'Operational' | 'System';

export interface SystemNotification {
  id: string;
  type: NotificationType;

  title: string;
  message: string;

  reference?: {
    kind: 'Freight' | 'Monitoring' | 'User';
    id: string;
  };

  createdAt: string;  // ISO
  timeLabel: string;  // "há 2 min" (visual)
  read: boolean;
}
