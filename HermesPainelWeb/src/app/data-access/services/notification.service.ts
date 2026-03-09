import { Injectable } from '@angular/core';
import { SystemNotification } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class NotificationsService {
  async list(): Promise<SystemNotification[]> {
    const now = new Date();

    const iso = (msAgo: number) => new Date(now.getTime() - msAgo).toISOString();

    return [
      {
        id: 'NT-001',
        type: 'NewFreight',
        title: 'Novo frete solicitado',
        message: 'FR-2010 • São Paulo → Rio de Janeiro • Aguardando aceite.',
        reference: { kind: 'Freight', id: 'FR-2010' },
        createdAt: iso(2 * 60_000),
        timeLabel: 'há 2 min',
        read: false,
      },
      {
        id: 'NT-002',
        type: 'Operational',
        title: 'Frete sem atualização',
        message: 'FR-1996 sem atualização há 3h. Verificar transportador e status.',
        reference: { kind: 'Monitoring', id: 'FR-1996' },
        createdAt: iso(10 * 60_000),
        timeLabel: 'há 10 min',
        read: false,
      },
      {
        id: 'NT-003',
        type: 'System',
        title: 'Aviso do sistema',
        message: 'Manutenção programada hoje às 23:00. Alguns serviços podem ficar instáveis.',
        createdAt: iso(60 * 60_000),
        timeLabel: 'há 1h',
        read: true,
      },
      {
        id: 'NT-004',
        type: 'Operational',
        title: 'Frete cancelado',
        message: 'FR-1979 foi cancelado. Verificar motivo e registrar ocorrência (se necessário).',
        reference: { kind: 'Freight', id: 'FR-1979' },
        createdAt: iso(26 * 60 * 60_000),
        timeLabel: 'há 1d',
        read: true,
      },
    ];
  }
}
