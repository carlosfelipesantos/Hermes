import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UsersService } from '../../data-access/services/users.service';
import { User, UserDetails, UserRole, UserStatus } from '../../data-access/models/user.model';

type RoleFilter = 'All' | UserRole;
type StatusFilter = 'All' | UserStatus;

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent {
  private service = inject(UsersService);

  loading = true;

  query = '';
  role: RoleFilter = 'All';
  status: StatusFilter = 'All';

  users: User[] = [];

  drawerOpen = false;
  detailsLoading = false;
  details: UserDetails | null = null;

  async ngOnInit() {
    this.users = await this.service.list();
    this.loading = false;
  }

  get filtered() {
    const q = this.query.trim().toLowerCase();

    return this.users.filter(u => {
      const matchQuery =
        !q ||
        u.name.toLowerCase().includes(q) ||
        u.email.toLowerCase().includes(q) ||
        u.id.toLowerCase().includes(q);

      const matchRole = this.role === 'All' || u.role === this.role;
      const matchStatus = this.status === 'All' || u.status === this.status;

      return matchQuery && matchRole && matchStatus;
    });
  }

  roleLabel(r: UserRole) {
    return r === 'Customer' ? 'Cliente' : 'Transportador';
  }

  statusLabel(s: UserStatus) {
    return s === 'Active' ? 'Ativo' : 'Inativo';
  }

  async open(u: User) {
    this.drawerOpen = true;
    this.detailsLoading = true;
    this.details = null;

    this.details = await this.service.getDetails(u.id);
    this.detailsLoading = false;
  }

  close() {
    this.drawerOpen = false;
    this.details = null;
  }

  // visual: alterna status no mock (depois vira API)
  toggleStatus() {
    if (!this.details) return;

    const newStatus: UserStatus = this.details.user.status === 'Active' ? 'Inactive' : 'Active';
    this.details = {
      ...this.details,
      user: { ...this.details.user, status: newStatus },
    };

    this.users = this.users.map(u =>
      u.id === this.details!.user.id ? { ...u, status: newStatus } : u
    );
  }

  toneClass(tone: 'neutral' | 'warn' | 'ok' | 'bad') {
    return { neutral: tone === 'neutral', warn: tone === 'warn', ok: tone === 'ok', bad: tone === 'bad' };
  }
}
