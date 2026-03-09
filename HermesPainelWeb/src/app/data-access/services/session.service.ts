import { Injectable, signal, computed } from '@angular/core';

export type UserProfile = {
  id: string;
  name: string;
  email: string;
  role: string;
  photoUrl?: string | null;
  avatarId?: string | null;
};

@Injectable({ providedIn: 'root' })
export class SessionService {
  private readonly _user = signal<UserProfile>({
    id: 'u-1',
    name: 'Hermes Admin',
    email: 'admin@hermes.local',
    role: 'Admin',
    photoUrl: 'assets/avatar.png',
    avatarId: null,
  });

  user = computed(() => this._user());

  updateProfile(data: Partial<Pick<UserProfile, 'name' | 'email'>>) {
    this._user.update(u => ({ ...u, ...data }));
  }

  setAvatar(avatarId: string) {
    this._user.update(u => ({ ...u, avatarId, photoUrl: null }));
  }

  setPhoto(photoUrl: string) {
    this._user.update(u => ({ ...u, photoUrl, avatarId: null }));
  }

  logout() {}
}
