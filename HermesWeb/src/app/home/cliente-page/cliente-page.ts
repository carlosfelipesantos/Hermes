import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Usuario } from '../../models/usuario/usuario.model';
import { FreteService } from '../../services/frete/frete.service';

@Component({
  selector: 'app-cliente-page',
  standalone: false,
  templateUrl: './cliente-page.html',
  styleUrls: ['./cliente-page.css']
})
export class ClientePage implements OnInit {
  usuario: Usuario | null = null;
  fretes: any[] = [];

  constructor(
    private authService: AuthService,
    private freteService: FreteService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.usuario = user;
    });
    this.carregarFretes();
   
    this.cdr.detectChanges();
  }

  carregarFretes(): void {
    this.freteService.getMeusFretes().subscribe({
      next: (data) => this.fretes = data,
      error: (err) => console.error('Erro ao carregar fretes', err)
    });
    
  }

  get nomeCliente(): string {
    if (!this.usuario) return 'Cliente';
    if (this.usuario.nome) {
      const primeiroNome = this.usuario.nome.split(' ')[0];
      return primeiroNome;
    }
    if (this.usuario.email) {
      return this.usuario.email.split('@')[0];
    }
    return 'Cliente';
  }
}