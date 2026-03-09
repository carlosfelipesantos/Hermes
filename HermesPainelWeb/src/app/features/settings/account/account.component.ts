import { Component, computed, inject, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SessionService } from '../../../data-access/services/session.service';
import { AvatarComponent } from '../../../shared/ui/avatar/avatar.component';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, AvatarComponent],
  templateUrl: './account.component.html',
  styleUrl: './account.component.css',
})
export class AccountComponent {
  session = inject(SessionService);

  saving = signal(false);
  saved = signal(false);
  editing = signal(false);
  tab = signal<'upload' | 'avatars'>('upload');

  name = signal(this.session.user().name);
  email = signal(this.session.user().email);

  avatars = [
    'robot-01','robot-02','robot-03','robot-04','robot-05','robot-06',
    'pilot-01','pilot-02','pilot-03','pilot-04','pilot-05','pilot-06',
  ];

  user = computed(() => this.session.user());

  openEditor() { this.editing.set(true); this.tab.set('upload'); }
  closeEditor() { this.editing.set(false); }

  saveProfile() {
    this.session.updateProfile({ name: this.name(), email: this.email() });
  }

  pickAvatar(id: string) {
    this.session.setAvatar(id);
    this.closeEditor();
  }

  constructor() {
    effect(() => {
      const u = this.user();
      this.name.set(u.name);
      this.email.set(u.email);
    });
  }


  async onFileSelected(ev: Event) {
    const input = ev.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const okType = ['image/png', 'image/jpeg', 'image/webp'].includes(file.type);
    if (!okType) { alert('Use PNG, JPG ou WEBP.'); input.value = ''; return; }
    if (file.size > 2_000_000) { alert('Arquivo muito grande (máx 2MB).'); input.value = ''; return; }

    const dataUrl = await this.readAsDataUrl(file);
    this.session.setPhoto(dataUrl);
    this.closeEditor();
  }

  private readAsDataUrl(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const r = new FileReader();
      r.onload = () => resolve(String(r.result));
      r.onerror = reject;
      r.readAsDataURL(file);
    });
  }
}
