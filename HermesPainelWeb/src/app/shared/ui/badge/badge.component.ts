import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

export type BadgeTone = 'neutral' | 'ok' | 'warn' | 'bad' | 'sys';

@Component({
  standalone: true,
  selector: 'ui-badge',
  imports: [CommonModule],
  template: `
    <span class="badge" [ngClass]="tone">
      <ng-content></ng-content>
    </span>
  `,
  styleUrls: ['./badge.component.css'],
})
export class BadgeComponent {
  @Input() tone: BadgeTone = 'neutral';
}
