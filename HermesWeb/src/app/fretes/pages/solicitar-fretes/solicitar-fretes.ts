import { Component } from '@angular/core';
import { FreteService } from '../../../services/frete/frete.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-solicitar-frete',
  templateUrl: './solicitar-fretes.html',
  styleUrls: ['./solicitar-fretes.css'],
  standalone: false
})
export class SolicitarFretes {
  // Origem
  cepOrigem = '';
  enderecoOrigem = '';
  bairroOrigem = '';
  cidadeOrigem = '';
  estadoOrigem = '';
  dddOrigem = '';
  carregandoCepOrigem = false;

  // Destino
  cepDestino = '';
  enderecoDestino = '';
  bairroDestino = '';
  cidadeDestino = '';
  estadoDestino = '';
  carregandoCepDestino = false;

  // Carga
  tipoCarga: 'Pequena' | 'Media' | 'Grande' | '' = '';
  valor = '';
  descricaoCarga = '';
  urgente = false;
  sitioOrigem = false;
  sitioDestino = false;

  // Estado
  enviando = false;
  sucesso = false;
  erro = '';

  constructor(
    private freteService: FreteService,
    private router: Router
  ) {}

  formatarCep(tipo: 'origem' | 'destino'): void {
    const campo = tipo === 'origem' ? 'cepOrigem' : 'cepDestino';
    let valor = (this[campo] as string).replace(/\D/g, '');
    if (valor.length > 5) valor = valor.slice(0, 5) + '-' + valor.slice(5, 8);
    (this[campo] as string) = valor;
  }

  buscarCep(tipo: 'origem' | 'destino'): void {
    const cep = (tipo === 'origem' ? this.cepOrigem : this.cepDestino).replace(/\D/g, '');
    if (cep.length !== 8) return;

    if (tipo === 'origem') this.carregandoCepOrigem = true;
    else this.carregandoCepDestino = true;

    fetch(`https://viacep.com.br/ws/${cep}/json/`)
      .then(r => r.json())
      .then(data => {
        if (!data.erro) {
          if (tipo === 'origem') {
            this.bairroOrigem = data.bairro;
            this.cidadeOrigem = data.localidade;
            this.estadoOrigem = data.uf;
            this.dddOrigem = data.ddd;
          } else {
            this.bairroDestino = data.bairro;
            this.cidadeDestino = data.localidade;
            this.estadoDestino = data.uf;
          }
        }
      })
      .finally(() => {
        if (tipo === 'origem') this.carregandoCepOrigem = false;
        else this.carregandoCepDestino = false;
      });
  }

  solicitarFrete(): void {
    this.erro = '';

    if (!this.cidadeOrigem || !this.estadoOrigem || !this.bairroOrigem) {
      this.erro = 'Preencha o endereço de origem completo.';
      return;
    }
    if (!this.cidadeDestino || !this.estadoDestino || !this.bairroDestino) {
      this.erro = 'Preencha o endereço de destino completo.';
      return;
    }
    if (!this.tipoCarga) {
      this.erro = 'Selecione o tipo de carga.';
      return;
    }
    if (!this.valor || !this.descricaoCarga) {
      this.erro = 'Preencha o valor e a descrição da carga.';
      return;
    }

    this.enviando = true;

    const payload = {
      tipoCarga: this.tipoCarga as 'Pequena' | 'Media' | 'Grande',
      status: 'Pendente' as const,
      urgente: this.urgente,

      dddOrigem: this.dddOrigem,
      cidadeOrigem: this.cidadeOrigem,
      bairroOrigem: this.bairroOrigem,
      estadoOrigem: this.estadoOrigem,
      latitudeOrigem: 0,
      longitudeOrigem: 0,

      cidadeDestino: this.cidadeDestino,
      bairroDestino: this.bairroDestino,
      estadoDestino: this.estadoDestino,
      latitudeDestino: 0,
      longitudeDestino: 0,

      descricaoCarga: this.descricaoCarga,
      valor: parseFloat(this.valor.replace('R$', '').replace('.', '').replace(',', '.').trim()),

      sitioOrigem: this.sitioOrigem,
      sitioDestino: this.sitioDestino,

      transportadorId: 0
    };

    this.freteService.criarFreteImediato(payload).subscribe({
      next: () => {
        this.sucesso = true;
        this.enviando = false;
        setTimeout(() => this.router.navigate(['/dashboard']), 2000);
      },
      error: (err: { error: { mensagem: string; }; }) => {
        this.erro = err?.error?.mensagem || 'Erro ao solicitar frete. Tente novamente.';
        this.enviando = false;
      }
    });
  }
}