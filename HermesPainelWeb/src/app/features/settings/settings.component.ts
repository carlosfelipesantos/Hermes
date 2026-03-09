import { Component } from '@angular/core';

@Component({
  standalone: true,
  template: `
    <div class="page">
      <h2>Configurações</h2>
      <p>Página visual (placeholder). Depois liga com API.</p>
    </div>
  `,
  styles: [`
    .page{background:#0f1a33;border:1px solid rgba(255,255,255,.08);border-radius:16px;padding:14px}
    h2{margin:0 0 6px;font-size:16px}
    p{margin:0;opacity:.75}
  `]
})
export class SettingsComponent {}
