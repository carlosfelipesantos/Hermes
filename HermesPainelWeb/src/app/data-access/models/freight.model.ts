export type FreightStatus = 'Requested' | 'Accepted' | 'InProgress' | 'Done' | 'Canceled';

export interface Freight {
  id: string;
  createdAt: string; 
  status: FreightStatus;

  origin: { city: string; address: string };
  destination: { city: string; address: string };

  cargo: { description: string; weightKg: number; volumeM3: number };
  price: number;

  transporter?: { name: string; vehicle: string; rating: number };
}
