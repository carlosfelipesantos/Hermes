import { Injectable } from '@angular/core';
import { Freight } from '../models/freight.model';

@Injectable({ providedIn: 'root' })
export class FreightService {
  async list(): Promise<Freight[]> {
    const now = new Date();
    const iso = (d: Date) => d.toISOString();

    return [
      {
        id: 'FR-2010',
        createdAt: iso(new Date(now.getTime() - 8 * 60_000)),
        status: 'Requested',
        origin: { city: 'São Paulo', address: 'Av. Paulista, 1000' },
        destination: { city: 'Rio de Janeiro', address: 'R. das Laranjeiras, 200' },
        cargo: { description: 'Caixas de eletrônicos', weightKg: 180, volumeM3: 2.4 },
        price: 1250,
      },
      {
        id: 'FR-2006',
        createdAt: iso(new Date(now.getTime() - 2 * 60 * 60_000)),
        status: 'Accepted',
        origin: { city: 'Campinas', address: 'R. Barão, 55' },
        destination: { city: 'Belo Horizonte', address: 'Av. Contorno, 900' },
        cargo: { description: 'Móveis desmontados', weightKg: 320, volumeM3: 6.1 },
        price: 1980,
        transporter: { name: 'João Silva', vehicle: 'VUC • 3/4', rating: 4.6 },
      },
      {
        id: 'FR-1996',
        createdAt: iso(new Date(now.getTime() - 6 * 60 * 60_000)),
        status: 'InProgress',
        origin: { city: 'Curitiba', address: 'Av. Sete de Setembro, 40' },
        destination: { city: 'Joinville', address: 'R. Blumenau, 12' },
        cargo: { description: 'Peças automotivas', weightKg: 240, volumeM3: 3.2 },
        price: 920,
        transporter: { name: 'Marcos Lima', vehicle: 'Fiorino', rating: 4.9 },
      },
      {
        id: 'FR-1988',
        createdAt: iso(new Date(now.getTime() - 2 * 24 * 60 * 60_000)),
        status: 'Done',
        origin: { city: 'Salvador', address: 'Av. Oceânica, 500' },
        destination: { city: 'Aracaju', address: 'R. Centro, 10' },
        cargo: { description: 'Carga frágil', weightKg: 90, volumeM3: 1.1 },
        price: 710,
        transporter: { name: 'Renato Costa', vehicle: 'Sprinter', rating: 4.7 },
      },
      {
        id: 'FR-1979',
        createdAt: iso(new Date(now.getTime() - 4 * 24 * 60 * 60_000)),
        status: 'Canceled',
        origin: { city: 'Porto Alegre', address: 'R. da Praia, 88' },
        destination: { city: 'Caxias do Sul', address: 'Av. Itália, 70' },
        cargo: { description: 'Materiais diversos', weightKg: 140, volumeM3: 2.0 },
        price: 560,
      },
    ];
  }
}
