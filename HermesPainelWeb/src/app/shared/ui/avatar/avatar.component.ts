import { Component, computed, input, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'ui-avatar',
  imports: [CommonModule],
  template: `
    <div class="ui-avatar" [style.width.px]="size()" [style.height.px]="size()">
      <ng-container *ngIf="src(); else noPhoto">
        <img [src]="src()!" alt="Foto de perfil" />
      </ng-container>

      <ng-template #noPhoto>
        <ng-container *ngIf="avatarUrl(); else initialsTpl">
          <img [src]="avatarUrl()!" alt="Avatar" />
        </ng-container>
        <ng-template #initialsTpl>
          <span class="initials">{{ initials() }}</span>
        </ng-template>
      </ng-template>
    </div>
  `,
  styles: [`
    .ui-avatar{
      border-radius: 14px;
      overflow: hidden;
      display: grid;
      place-items: center;
      background: rgba(34, 197, 94, 0.18);
      border: 1px solid rgba(34, 197, 94, 0.30);
      color: #22c55e;
      font-weight: 900;
    }
    img{ width:100%; height:100%; object-fit: cover; display:block; }
    .initials{ font-size: 14px; }
  `]
})
export class AvatarComponent {
  name = input<string>('');
  src = input<string | null>(null);       
  avatarId = input<string | null>(null);  
  size = input<number>(36);

  initials = computed(() => {
    const n = (this.name() || '').trim();
    if (!n) return 'U';
    const parts = n.split(/\s+/).slice(0, 2);
    return parts.map(p => p[0]?.toUpperCase()).join('');
  });

  avatarUrl = computed(() => {
    const id = this.avatarId();
    return id ? `/assets/avatars/${id}.png` : null;
  });
}
