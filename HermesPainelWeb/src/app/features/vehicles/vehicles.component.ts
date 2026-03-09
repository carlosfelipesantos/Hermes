import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { VehiclesService } from '../../data-access/services/vehicle.service';
import { Vehicle, VehicleAvailability, VehicleType } from '../../data-access/models/vehicle.model';

type TypeFilter = 'All' | VehicleType;
type AvailFilter = 'All' | VehicleAvailability;

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css'],
})
export class VehiclesComponent {
  private service = inject(VehiclesService);

  loading = true;

  query = '';
  type: TypeFilter = 'All';
  availability: AvailFilter = 'All';
  transporter = 'All';

  vehicles: Vehicle[] = [];
  selected: Vehicle | null = null;

  async ngOnInit() {
    this.vehicles = await this.service.list();
    this.loading = false;
  }

  get transporters() {
    const names = Array.from(new Set(this.vehicles.map(v => v.transporterName))).sort();
    return ['All', ...names];
  }

  get filtered() {
    const q = this.query.trim().toLowerCase();

    return this.vehicles.filter(v => {
      const matchQuery =
        !q ||
        v.id.toLowerCase().includes(q) ||
        v.plate.toLowerCase().includes(q) ||
        v.transporterName.toLowerCase().includes(q);

      const matchType = this.type === 'All' || v.type === this.type;
      const matchAvail = this.availability === 'All' || v.availability === this.availability;
      const matchTransporter = this.transporter === 'All' || v.transporterName === this.transporter;

      return matchQuery && matchType && matchAvail && matchTransporter;
    });
  }

  open(v: Vehicle) { this.selected = v; }
  close() { this.selected = null; }

  availLabel(a: VehicleAvailability) {
    switch (a) {
      case 'Available': return 'Disponível';
      case 'Busy': return 'Em uso';
      case 'Inactive': return 'Inativo';
    }
  }

  availClass(a: VehicleAvailability) {
    return { ok: a === 'Available', warn: a === 'Busy', bad: a === 'Inactive' };
  }

  formatCapacity(kg: number) {
    return `${kg.toLocaleString('pt-BR')} kg`;
  }

  formatVolume(m3: number) {
    return `${m3.toLocaleString('pt-BR')} m³`;
  }
}
