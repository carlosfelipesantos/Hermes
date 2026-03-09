import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NotificationsService } from '../../data-access/services/notification.service';
import { NotificationType, SystemNotification } from '../../data-access/models/notification.model';
import { BadgeComponent } from '../../shared/ui/badge/badge.component';
import { DrawerComponent } from '../../shared/ui/drawer/drawer.component';

type TypeFilter = 'All' | NotificationType;
type ReadFilter = 'All' | 'Unread' | 'Read';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, BadgeComponent, DrawerComponent],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent {
  private service = inject(NotificationsService);

  loading = true;

  query = '';
  type: TypeFilter = 'All';
  read: ReadFilter = 'All';

  items: SystemNotification[] = [];
  selected: SystemNotification | null = null;

  async ngOnInit() {
    this.items = await this.service.list();
    this.loading = false;
  }

  get filtered() {
    const q = this.query.trim().toLowerCase();

    return this.items.filter(n => {
      const matchQuery =
        !q ||
        n.title.toLowerCase().includes(q) ||
        n.message.toLowerCase().includes(q) ||
        (n.reference?.id?.toLowerCase().includes(q) ?? false);

      const matchType = this.type === 'All' || n.type === this.type;
      const matchRead =
        this.read === 'All' ||
        (this.read === 'Unread' && !n.read) ||
        (this.read === 'Read' && n.read);

      return matchQuery && matchType && matchRead;
    });
  }

  open(n: SystemNotification) {
    this.selected = n;
    // visual: marcar como lida ao abrir
    this.items = this.items.map(x => (x.id === n.id ? { ...x, read: true } : x));
    this.selected = { ...n, read: true };
  }

  close() {
    this.selected = null;
  }

  markAllRead() {
    this.items = this.items.map(n => ({ ...n, read: true }));
  }

  typeLabel(t: NotificationType) {
    switch (t) {
      case 'NewFreight': return 'Novo frete';
      case 'Operational': return 'Operacional';
      case 'System': return 'Sistema';
    }
  }

  typeClass(t: NotificationType) {
    return { ok: t === 'NewFreight', warn: t === 'Operational', sys: t === 'System' };
  }

  refLink(n: SystemNotification) {
    if (!n.reference) return null;
    if (n.reference.kind === 'Freight') return '/freights';
    if (n.reference.kind === 'Monitoring') return '/monitoring';
    if (n.reference.kind === 'User') return '/users';
    return null;
  }

  refLabel(n: SystemNotification) {
    if (!n.reference) return '—';
    if (n.reference.kind === 'Freight') return `Frete ${n.reference.id}`;
    if (n.reference.kind === 'Monitoring') return `Monitoramento ${n.reference.id}`;
    if (n.reference.kind === 'User') return `Usuário ${n.reference.id}`;
    return n.reference.id;
  }
}
