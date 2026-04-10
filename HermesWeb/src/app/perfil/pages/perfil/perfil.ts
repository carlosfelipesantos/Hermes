import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UsuarioService } from '../../../services/usuario/usuario.service';
import { FreteService } from '../../../services/frete/frete.service';
import { AvaliacaoService } from '../../../services/avaliação/avaliacao.service';
import { AuthService } from '../../../services/auth/auth.service';
import { Usuario } from '../../../models/usuario/usuario.model';
import { Frete } from '../../../models/fretes/fretes.model';
import { Avaliacao } from '../../../models/avaliacao/avaliacao.model';
import { Veiculo } from '../../../models/veiculo/veiculo.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  styleUrls: ['./perfil.css']
})
export class PerfilComponent implements OnInit {
  usuario: Usuario | null = null;
  historicoFretes: Frete[] = [];
  avaliacoesRecebidas: Avaliacao[] = [];
  avaliacoesFeitas: Avaliacao[] = [];
  mediaAvaliacoes: number | null = null;
  loading = true;
  salvo = false;

  perfilForm!: FormGroup;
  modalExclusaoAberto = false;
  confirmacaoTexto = '';
  excluirLoading = false;

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private freteService: FreteService,
    private avaliacaoService: AvaliacaoService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.loading = true;
    this.usuarioService.getMe().subscribe({
      next: (user) => {
        this.usuario = user;
        this.inicializarForm();
        this.carregarHistoricoFretes();
        this.carregarAvaliacoes();
      },
      error: (err) => {
        console.error('Erro ao carregar perfil', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  inicializarForm(): void {
    if (!this.usuario) return;
    this.perfilForm = this.fb.group({
      nome: [this.usuario.nome, Validators.required],
      email: [this.usuario.email, [Validators.required, Validators.email]],
      telefone: [this.usuario.telefone, Validators.required],
      estado: [this.usuario.estado],
      cidade: [this.usuario.cidade],
      endereco: [this.usuario.endereco],
      documento: [this.usuario.documento],
      ddd: [{ value: this.usuario.ddd, disabled: true }]
    });
  }

  carregarHistoricoFretes(): void {
    if (!this.usuario) return;
    if (this.usuario.tipo === 'Cliente') {
      this.freteService.getMeusFretes().subscribe({
        next: (fretes) => { this.historicoFretes = fretes; this.loading = false; this.cdr.detectChanges(); },
        error: (err) => { console.error(err); this.loading = false; this.cdr.detectChanges(); }
      });
    } else {
      this.freteService.getMeusTransportes().subscribe({
        next: (fretes) => { this.historicoFretes = fretes; this.loading = false; this.cdr.detectChanges(); },
        error: (err) => { console.error(err); this.loading = false; this.cdr.detectChanges(); }
      });
    }
  }

  carregarAvaliacoes(): void {
    if (!this.usuario) return;
    if (this.usuario.tipo === 'Transportador' && this.usuario.id) {
      this.avaliacaoService.getAvaliacoesRecebidas(this.usuario.id).subscribe({
        next: (avals) => {
          this.avaliacoesRecebidas = avals;
          this.calcularMedia();
        },
        error: (err) => console.error(err)
      });
    } else if (this.usuario.tipo === 'Cliente' && this.usuario.id) {
      this.avaliacaoService.getMinhasAvaliacoes(this.usuario.id).subscribe({
        next: (avals) => this.avaliacoesFeitas = avals,
        error: (err) => console.error(err)
      });
    }
  }

  calcularMedia(): void {
    if (this.avaliacoesRecebidas.length === 0) {
      this.mediaAvaliacoes = null;
      return;
    }
    const soma = this.avaliacoesRecebidas.reduce((acc, a) => acc + a.nota, 0);
    this.mediaAvaliacoes = soma / this.avaliacoesRecebidas.length;
  }

  onSubmit(): void {
    if (this.perfilForm.invalid) return;
    if (!this.usuario) return;

    const dadosAtualizados = { ...this.usuario, ...this.perfilForm.getRawValue() };
    this.usuarioService.atualizarPerfil(dadosAtualizados).subscribe({
      next: (atualizado) => {
        this.usuario = atualizado;
        this.salvo = true;
        setTimeout(() => this.salvo = false, 3000);
        this.cdr.detectChanges();
      },
      error: (err: any[]) => console.error('Erro ao atualizar', err)
    });
  }

  abrirModalExclusao(): void {
    this.modalExclusaoAberto = true;
    this.confirmacaoTexto = '';
    this.excluirLoading = false;
    document.body.classList.add('modal-open');
  }

  fecharModalExclusao(): void {
    this.modalExclusaoAberto = false;
    document.body.classList.remove('modal-open');
  }

  confirmarExclusao(): void {
    if (this.confirmacaoTexto !== 'EXCLUIR CONTA') return;
    this.excluirLoading = true;
    this.usuarioService.deleteConta().subscribe({
      next: () => {
        alert('Conta excluída com sucesso!');
        this.authService.logout();
        this.router.navigate(['/']);
      },
      error: (err: any[]) => {
        console.error(err);
        alert('Erro ao excluir conta. Tente novamente.');
        this.excluirLoading = false;
      }
    });
  }

  get inicial(): string {
    return this.usuario?.nome ? this.usuario.nome.charAt(0).toUpperCase() : '?';
  }

  get tipoUsuarioLabel(): string {
    return this.usuario?.tipo === 'Transportador' ? 'Transportador' : 'Cliente';
  }

  get dddExibido(): string {
    return this.usuario?.ddd || '--';
  }

  get enderecoCompleto(): string {
    const { cidade, estado } = this.usuario || {};
    if (!cidade && !estado) return 'Localização não informada';
    return `${cidade || ''}${cidade && estado ? ' - ' : ''}${estado || ''}`;
  }
}