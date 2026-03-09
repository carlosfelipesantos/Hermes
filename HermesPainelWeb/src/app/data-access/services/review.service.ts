import { Injectable } from '@angular/core';
import { Review, ReviewsDashboardData } from '../models/review.model';

@Injectable({ providedIn: 'root' })
export class ReviewsService {
  async getDashboard(): Promise<ReviewsDashboardData> {
    const latestReviews: Review[] = [
      {
        id: 'RV-001',
        freightId: 'FR-1988',
        transporterId: 'U-1002',
        transporterName: 'Marcos Lima',
        customerName: 'Ana Souza',
        rating: 5,
        comment: 'Chegou no prazo e cuidou muito bem da carga.',
        createdAt: new Date().toISOString(),
        dateLabel: '11/02 21:10',
      },
      {
        id: 'RV-002',
        freightId: 'FR-2006',
        transporterId: 'U-1001',
        transporterName: 'João Silva',
        customerName: 'Pedro Alves',
        rating: 4,
        comment: 'Boa comunicação, mas atrasou um pouco.',
        createdAt: new Date().toISOString(),
        dateLabel: '11/02 18:40',
      },
      {
        id: 'RV-003',
        freightId: 'FR-1970',
        transporterId: 'U-1003',
        transporterName: 'Renato Costa',
        customerName: 'Luiza Martins',
        rating: 5,
        comment: 'Excelente atendimento e veículo adequado.',
        createdAt: new Date().toISOString(),
        dateLabel: '09/02 10:15',
      },
    ];

    return {
      averageRating: 4.7,
      totalReviews: 312,
      ratedTransporters: 58,
      topTransporters: [
        {
          transporterId: 'U-1002',
          transporterName: 'Marcos Lima',
          average: 4.9,
          totalReviews: 86,
          lastComment: 'Chegou no prazo e cuidou muito bem da carga.',
        },
        {
          transporterId: 'U-1003',
          transporterName: 'Renato Costa',
          average: 4.8,
          totalReviews: 64,
          lastComment: 'Excelente atendimento e veículo adequado.',
        },
        {
          transporterId: 'U-1001',
          transporterName: 'João Silva',
          average: 4.6,
          totalReviews: 52,
          lastComment: 'Boa comunicação, mas atrasou um pouco.',
        },
      ],
      latestReviews,
    };
  }

  async getTransporterReviews(transporterId: string): Promise<Review[]> {
    // mock simples: retorna alguns itens
    const all = (await this.getDashboard()).latestReviews;

    if (all.some(r => r.transporterId === transporterId)) {
      return all.filter(r => r.transporterId === transporterId);
    }

    return [
      {
        id: 'RV-010',
        freightId: 'FR-1901',
        transporterId,
        transporterName: 'Transportador',
        customerName: 'Cliente',
        rating: 5,
        comment: 'Ótimo serviço.',
        createdAt: new Date().toISOString(),
        dateLabel: '08/02 09:20',
      },
    ];
  }
}
