import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { BadgeComponent } from '../../shared/ui/badge/badge.component';
import { DrawerComponent } from '../../shared/ui/drawer/drawer.component';

import { Freight, FreightStatus } from '../../data-access/models/freight.model';
import { FreightService } from '../../data-access/services/freight.service';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';


type TimelineStep = { key: FreightStatus; label: string };

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, BadgeComponent, DrawerComponent, RouterLink, RouterOutlet, RouterLinkActive],
  templateUrl: './freights.component.html',
  styleUrls: ['./freights.component.css'],
})
export class FreightsComponent {
  private service = inject(FreightService);

  loading = true;

  query = '';
  status: 'All' | FreightStatus = 'All';

  freights: Freight[] = [];
  selected: Freight | null = null;

  async ngOnInit() {
    this.freights = await this.service.list();
    this.loading = false;
  }

  openDetails(f: Freight) { this.selected = f; }
  closeDetails() { this.selected = null; }

  get filtered(): Freight[] {
    const q = this.query.trim().toLowerCase();
    return this.freights.filter((f) => {
      const matchQuery =
        !q ||
        f.id.toLowerCase().includes(q) ||
        f.origin.city.toLowerCase().includes(q) ||
        f.destination.city.toLowerCase().includes(q) ||
        (f.transporter?.name?.toLowerCase().includes(q) ?? false);

      const matchStatus = this.status === 'All' || f.status === this.status;

      return matchQuery && matchStatus;
    });
  }

  money(v: number) {
    return v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  statusLabel(s: FreightStatus) {
    switch (s) {
      case 'Requested': return 'Solicitado';
      case 'Accepted': return 'Aceito';
      case 'InProgress': return 'Em andamento';
      case 'Done': return 'Finalizado';
      case 'Canceled': return 'Cancelado';
    }
  }

  badgeTone(s: FreightStatus) {
    switch (s) {
      case 'Done': return 'ok';
      case 'Accepted':
      case 'InProgress': return 'warn';
      case 'Canceled': return 'bad';
      default: return 'neutral';
    }
  }

  timelineSteps(current: FreightStatus): TimelineStep[] {
  const steps: TimelineStep[] = [
    { key: 'Requested',  label: 'Solicitado' },
    { key: 'Accepted',   label: 'Aceito' },
    { key: 'InProgress', label: 'Em andamento' },
    { key: 'Done',       label: 'Finalizado' },
  ];

  if (current === 'Canceled') {
    return [...steps, { key: 'Canceled', label: 'Cancelado' }];
  }

  return steps;
}

  isStepDone(step: FreightStatus, current: FreightStatus) {
    const order: FreightStatus[] = ['Requested', 'Accepted', 'InProgress', 'Done'];
    if (current === 'Canceled') return step === 'Canceled';
    return order.indexOf(step) <= order.indexOf(current);
  }

  isStepCurrent(step: FreightStatus, current: FreightStatus) {
    return step === current;
  }
}
