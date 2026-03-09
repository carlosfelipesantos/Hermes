import { Injectable } from '@angular/core';
import { User, UserDetails } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class UsersService {
  async list(): Promise<User[]> {
    const now = new Date().toISOString();

    return [
      {
        id: 'U-1001',
        name: 'João Silva',
        email: 'joao@hermes.com',
        phone: '(11) 99999-1111',
        role: 'Transporter',
        status: 'Active',
        createdAt: now,
        rating: 4.6,
        activeVehicles: 2,
      },
      {
        id: 'U-1002',
        name: 'Marcos Lima',
        email: 'marcos@hermes.com',
        phone: '(41) 98888-2222',
        role: 'Transporter',
        status: 'Active',
        createdAt: now,
        rating: 4.9,
        activeVehicles: 1,
      },
      {
        id: 'U-2001',
        name: 'Ana Souza',
        email: 'ana@hermes.com',
        phone: '(31) 97777-3333',
        role: 'Customer',
        status: 'Inactive',
        createdAt: now,
      },
      {
        id: 'U-2002',
        name: 'Pedro Alves',
        email: 'pedro@hermes.com',
        phone: '(21) 96666-4444',
        role: 'Customer',
        status: 'Active',
        createdAt: now,
      },
    ];
  }

  async getDetails(userId: string): Promise<UserDetails> {
    // mock simples baseado no id
    const base = (await this.list()).find(u => u.id === userId) ?? (await this.list())[0];

    return {
      user: base,
      stats: {
        totalFreights: base.role === 'Transporter' ? 34 : 6,
        doneFreights: base.role === 'Transporter' ? 29 : 4,
        canceledFreights: base.role === 'Transporter' ? 2 : 1,
      },
      history: [
        { id: 'FR-2010', statusLabel: 'Solicitado', statusTone: 'neutral', origin: 'São Paulo', destination: 'Rio', valueLabel: 'R$ 1.250,00', dateLabel: '11/02 21:10' },
        { id: 'FR-2006', statusLabel: 'Aceito', statusTone: 'warn', origin: 'Campinas', destination: 'BH', valueLabel: 'R$ 1.980,00', dateLabel: '11/02 18:40' },
        { id: 'FR-1988', statusLabel: 'Finalizado', statusTone: 'ok', origin: 'Salvador', destination: 'Aracaju', valueLabel: 'R$ 710,00', dateLabel: '09/02 10:15' },
      ],
    };
  }
}
