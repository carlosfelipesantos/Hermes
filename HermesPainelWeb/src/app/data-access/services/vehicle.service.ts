import { Injectable } from '@angular/core';
import { Vehicle } from '../models/vehicle.model';

@Injectable({ providedIn: 'root' })
export class VehiclesService {
  async list(): Promise<Vehicle[]> {
    const now = new Date().toISOString();

    return [
      {
        id: 'V-3001',
        plate: 'ABC1D23',
        type: 'Van',
        capacityKg: 1200,
        volumeM3: 8.5,
        availability: 'Available',
        transporterId: 'U-1002',
        transporterName: 'Marcos Lima',
        createdAt: now,
        notes: 'Seguro em dia • Baú fechado',
      },
      {
        id: 'V-3002',
        plate: 'DEF4G56',
        type: 'VUC',
        capacityKg: 2500,
        volumeM3: 14.0,
        availability: 'Busy',
        transporterId: 'U-1001',
        transporterName: 'João Silva',
        createdAt: now,
        notes: 'Rastreio ativo',
      },
      {
        id: 'V-3003',
        plate: 'HIJ7K89',
        type: 'Caminhao',
        capacityKg: 8000,
        volumeM3: 32.0,
        availability: 'Inactive',
        transporterId: 'U-1001',
        transporterName: 'João Silva',
        createdAt: now,
        notes: 'Em manutenção',
      },
      {
        id: 'V-3004',
        plate: 'MNO0P12',
        type: 'Moto',
        capacityKg: 35,
        volumeM3: 0.2,
        availability: 'Available',
        transporterId: 'U-1002',
        transporterName: 'Marcos Lima',
        createdAt: now,
      },
    ];
  }
}
