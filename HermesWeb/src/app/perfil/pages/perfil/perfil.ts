import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
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
    private cdr: ChangeDetectorRef,
    private router: Router
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

  getDashboardLink(): string {
    if (!this.usuario) return '/';
    switch (this.usuario.tipo) {
      case 'Cliente':
        return '/cliente';
      case 'Transportador':
        return '/transportador';
      case 'Admin':
        return '/admin/dashboard';  
      default: return '/';
    }
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

  onFotoSelecionada(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    const formData = new FormData();
    formData.append('foto', file);

    this.usuarioService.uploadFoto(formData).subscribe({
      next: (response: { fotoUrl: any; }) => {
        this.usuario.fotoPerfil = response.fotoUrl;
        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Erro ao enviar foto', err)
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