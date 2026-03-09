import { Injectable } from '@angular/core';
import { MonitoringItem } from '../models/monitoring.model';

@Injectable({ providedIn: 'root' })
export class MonitoringService {
  async list(): Promise<MonitoringItem[]> {
    return [
      {
        id: 'FR-1996',
        origin: 'Curitiba',
        destination: 'Joinville',
        status: 'InProgress',
        lastUpdateLabel: 'há 6 min',
        riskLabel: 'Ok',
        transporter: 'Marcos Lima',
        valueLabel: 'R$ 920,00',
        cargo: 'Peças automotivas (240kg)',
      },
      {
        id: 'FR-2002',
        origin: 'Campinas',
        destination: 'Belo Horizonte',
        status: 'Stopped',
        lastUpdateLabel: 'há 3h',
        riskLabel: 'Attention',
        transporter: 'João Silva',
        valueLabel: 'R$ 1.980,00',
        cargo: 'Móveis desmontados (320kg)',
      },
      {
        id: 'FR-1979',
        origin: 'Porto Alegre',
        destination: 'Caxias do Sul',
        status: 'Canceled',
        lastUpdateLabel: 'há 1d',
        riskLabel: 'Critical',
        transporter: '—',
        valueLabel: 'R$ 560,00',
        cargo: 'Materiais diversos (140kg)',
      },
    ];
  }
}
