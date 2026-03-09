import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MonitoringService } from '../../data-access/services/monitoring.service';
import { MonitoringItem, MonitoringStatus } from '../../data-access/models/monitoring.model';

type StatusFilter = 'All' | MonitoringStatus;

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './monitoring.component.html',
  styleUrls: ['./monitoring.component.css'],
})
export class MonitoringComponent {
  private service = inject(MonitoringService);

  loading = true;

  query = '';
  status: StatusFilter = 'All';

  items: MonitoringItem[] = [];
  selected: MonitoringItem | null = null;

  async ngOnInit() {
    this.items = await this.service.list();
    this.loading = false;
  }

  open(item: MonitoringItem) {
    this.selected = item;
  }

  close() {
    this.selected = null;
  }

  get filtered() {
    const q = this.query.trim().toLowerCase();

    return this.items.filter(i => {
      const matchQuery =
        !q ||
        i.id.toLowerCase().includes(q) ||
        i.origin.toLowerCase().includes(q) ||
        i.destination.toLowerCase().includes(q) ||
        i.transporter.toLowerCase().includes(q);

      const matchStatus = this.status === 'All' || i.status === this.status;
      return matchQuery && matchStatus;
    });
  }

  get kpis() {
    const inProgress = this.items.filter(i => i.status === 'InProgress').length;
    const stopped = this.items.filter(i => i.status === 'Stopped').length;
    const canceled = this.items.filter(i => i.status === 'Canceled').length;
    return { inProgress, stopped, canceled };
  }

  statusLabel(s: MonitoringStatus) {
    switch (s) {
      case 'InProgress': return 'Em andamento';
      case 'Stopped': return 'Parado';
      case 'Canceled': return 'Cancelado';
    }
  }

  statusClass(s: MonitoringStatus) {
    return { ok: s === 'InProgress', warn: s === 'Stopped', bad: s === 'Canceled' };
  }

  riskClass(r: MonitoringItem['riskLabel']) {
    return { ok: r === 'Ok', warn: r === 'Attention', bad: r === 'Critical' };
  }
}
