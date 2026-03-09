export type VehicleType = 'Moto' | 'Van' | 'VUC' | 'Caminhao';
export type VehicleAvailability = 'Available' | 'Busy' | 'Inactive';

export interface Vehicle {
  id: string;         
  plate: string;        
  type: VehicleType;

  capacityKg: number;
  volumeM3: number;

  availability: VehicleAvailability;

  transporterId: string;
  transporterName: string;

  createdAt: string; 
  notes?: string;
}
