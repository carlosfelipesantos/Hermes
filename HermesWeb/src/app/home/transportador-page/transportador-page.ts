import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Usuario } from '../../models/usuario/usuario.model';
import { FreteService } from '../../services/frete/frete.service';
import { VeiculoService } from '../../services/veiculo/veiculo.service';
import { DisponibilidadeService } from '../../services/disponibilidade/disponibilidade.service';
import { HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-transportador-page',
  standalone: false,
  templateUrl: './transportador-page.html',
  styleUrls: ['./transportador-page.css']
})
export class TransportadorPage implements OnInit {
  usuario: Usuario | null = null;
  fretesAceitos: any[] = [];
  fretesDisponiveis: any[] = [];
  veiculos: any[] = [];
  disponibilidades: any[] = [];
  loading = true;

  constructor(
    private authService: AuthService,
    private freteService: FreteService,
    private veiculoService: VeiculoService,
    private disponibilidadeService: DisponibilidadeService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.usuario = user;
    });
    this.carregarDados();
    this.cdr.detectChanges();
  }

  get nomeTransportador(): string {
    if (!this.usuario) return 'Transportador';
    if (this.usuario.nome) return this.usuario.nome.split(' ')[0];
    if (this.usuario.email) return this.usuario.email.split('@')[0];
    return 'Transportador';
  }

  carregarDados(): void {
    this.loading = true;
    const token = this.authService.getToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.freteService.getMeusTransportes().subscribe({
      next: (data) => this.fretesAceitos = data,
      error: (err) => console.error('Erro ao carregar meus transportes', err),
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.freteService.getFretesDisponiveis().subscribe({
      next: (data) => this.fretesDisponiveis = data,
      error: (err) => console.error('Erro ao carregar fretes disponíveis', err),
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.veiculoService.getMeusVeiculos().subscribe({
      next: (data) => this.veiculos = data,
      error: (err) => console.error('Erro ao carregar veículos', err),
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.disponibilidadeService.getMinhasDisponibilidades().subscribe({
      next: (data) => this.disponibilidades = data,
      error: (err) => console.error('Erro ao carregar disponibilidade', err),
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

  }
}