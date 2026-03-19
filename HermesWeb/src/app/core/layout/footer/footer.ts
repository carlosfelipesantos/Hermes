import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: false,
  templateUrl: './footer.html',
  styleUrl: './footer.css'
})
export class Footer {
  emailNewsletter = '';

  assinarNewsletter(): void {
    console.log('E-mail cadastrado na newsletter:', this.emailNewsletter);
    this.emailNewsletter = '';
  }
}