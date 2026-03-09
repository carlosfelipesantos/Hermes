import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  standalone: true,
  selector: 'ui-drawer',
  imports: [CommonModule],
  templateUrl: './drawer.component.html',
  styleUrls: ['./drawer.component.css'],
})
export class DrawerComponent {
  @Input() open = false;
  @Input() width = 440; // px

  @Output() closed = new EventEmitter<void>();

  close() {
    this.closed.emit();
  }
}
