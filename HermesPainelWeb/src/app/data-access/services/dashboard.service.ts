import { Injectable } from '@angular/core';
import { DashboardData } from '../models/dashboard.model';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  async get(): Promise<DashboardData> {
    return {
      kpis: {
        requestedFreights: 28,
        inProgressFreights: 14,
        doneFreights: 102,
        usersCount: 482,
        activeTransporters: 67,
        averageRating: 4.7,
      },
      latestFreights: [
        {
          id: 'FR-2010',
          statusLabel: 'Solicitado',
          statusTone: 'neutral',
          origin: 'São Paulo',
          destination: 'Rio de Janeiro',
          valueLabel: 'R$ 1.250,00',
          transporterLabel: '—',
        },
        {
          id: 'FR-2006',
          statusLabel: 'Aceito',
          statusTone: 'warn',
          origin: 'Campinas',
          destination: 'Belo Horizonte',
          valueLabel: 'R$ 1.980,00',
          transporterLabel: 'João Silva',
        },
        {
          id: 'FR-1996',
          statusLabel: 'Em andamento',
          statusTone: 'warn',
          origin: 'Curitiba',
          destination: 'Joinville',
          valueLabel: 'R$ 920,00',
          transporterLabel: 'Marcos Lima',
        },
        {
          id: 'FR-1988',
          statusLabel: 'Finalizado',
          statusTone: 'ok',
          origin: 'Salvador',
          destination: 'Aracaju',
          valueLabel: 'R$ 710,00',
          transporterLabel: 'Renato Costa',
        },
      ],
      alerts: [
        {
          id: 'AL-01',
          type: 'NewFreight',
          title: 'Novo frete solicitado',
          description: 'FR-2010 • São Paulo → Rio de Janeiro',
          timeLabel: 'há 2 min',
        },
        {
          id: 'AL-02',
          type: 'Operational',
          title: 'Frete sem atualização',
          description: 'FR-1996 parado há 3h (verificar transportador)',
          timeLabel: 'há 10 min',
        },
        {
          id: 'AL-03',
          type: 'System',
          title: 'Aviso do sistema',
          description: 'Manutenção programada hoje às 23:00',
          timeLabel: 'há 1h',
        },
      ],
    };
  }
}
