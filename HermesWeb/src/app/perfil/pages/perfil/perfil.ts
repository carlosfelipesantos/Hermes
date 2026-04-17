import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { UsuarioService } from '../../../services/usuario/usuario.service';
import { Usuario } from '../../../models/usuario/usuario.model';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  styleUrls: ['./perfil.css'],
  standalone: false
})
export class Perfil implements OnInit {
  usuario: Usuario = {} as Usuario;
  sucesso = false;

  modalExcluirAberto = false;
  confirmacaoTexto = '';
  excluindoConta = false;

  constructor(
    private usuarioService: UsuarioService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.carregarUsuario();
  }

  carregarUsuario(): void {
    console.log('chamando getMe...');
    this.usuarioService.getMe().subscribe({
      next: (response) => {
        console.log('getMe response', response);
        this.usuario = { ...response };
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('getMe erro', err);
      }
    });
  }

  salvarPerfil(): void {
    this.usuarioService.atualizarPerfil(this.usuario).subscribe({
      next: (response) => {
        this.usuario = { ...response };
        this.sucesso = true;
        this.cdr.detectChanges();

        setTimeout(() => {
          this.sucesso = false;
          this.cdr.detectChanges();
        }, 3000);
      },
      error: (err) => {
        console.error('Erro ao atualizar perfil', err);
      }
    });
  }

  abrirModalExclusao(): void {
    this.modalExcluirAberto = true;
  }

  fecharModalExclusao(): void {
    this.modalExcluirAberto = false;
    this.confirmacaoTexto = '';
    this.excluindoConta = false;
  }

  podeExcluir(): boolean {
    return this.confirmacaoTexto.trim().toUpperCase() === 'EXCLUIR CONTA';
  }

  confirmarExclusaoConta(): void {
    if (!this.podeExcluir()) return;

    this.excluindoConta = true;

    this.usuarioService.deleteConta().subscribe({
      next: () => {
        localStorage.clear();
        window.location.href = '/';
      },
      error: (err) => {
        console.error('Erro ao excluir conta', err);
        this.excluindoConta = false;
        this.cdr.detectChanges();
      }
    });
  }

  isTransportador(): boolean {
    return (this.usuario?.tipo || '').toLowerCase() === 'transportador';
  }

  get inicial(): string {
    return this.usuario?.nome?.trim()?.charAt(0)?.toUpperCase() || '';
  }

  get localizacao(): string {
    const cidade = this.usuario?.cidade || '';
    const estado = this.usuario?.estado || '';

    if (!cidade && !estado) return 'Localização não informada';
    if (cidade && estado) return `${cidade} - ${estado}`;
    return cidade || estado;
  }
}