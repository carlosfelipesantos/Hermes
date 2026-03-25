import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-solicitar-fretes',
  standalone: false,
  templateUrl: './solicitar-fretes.html',
  styleUrl: './solicitar-fretes.css',
})
export class SolicitarFretes {
cepOrigem = '';
  enderecoOrigem = '';
  cidadeOrigem = '';
  estadoOrigem = '';

  cepDestino = '';
  enderecoDestino = '';
  cidadeDestino = '';
  estadoDestino = '';

  carregandoCepOrigem = false;
  carregandoCepDestino = false;

  formatarCep(tipo: 'origem' | 'destino'): void {
    if (tipo === 'origem') {
      this.cepOrigem = this.aplicarMascaraCep(this.cepOrigem);
      return;
    }

    this.cepDestino = this.aplicarMascaraCep(this.cepDestino);
  }

  aplicarMascaraCep(valor: string): string {
    const numeros = valor.replace(/\D/g, '').slice(0, 8);

    if (numeros.length <= 5) {
      return numeros;
    }

    return `${numeros.slice(0, 5)}-${numeros.slice(5)}`;
  }

  async buscarCep(tipo: 'origem' | 'destino'): Promise<void> {
    const cep = tipo === 'origem' ? this.cepOrigem : this.cepDestino;
    const cepLimpo = cep.replace(/\D/g, '');

    if (cepLimpo.length !== 8) {
      return;
    }

    if (tipo === 'origem') {
      this.carregandoCepOrigem = true;
    } else {
      this.carregandoCepDestino = true;
    }

    try {
      const resposta = await fetch(`https://viacep.com.br/ws/${cepLimpo}/json/`);
      const dados = await resposta.json();

      if (dados.erro) {
        return;
      }

      const enderecoCompleto = [dados.logradouro, dados.bairro]
        .filter(Boolean)
        .join(' - ');

      if (tipo === 'origem') {
        this.enderecoOrigem = enderecoCompleto;
        this.cidadeOrigem = dados.localidade || '';
        this.estadoOrigem = dados.uf || '';
      } else {
        this.enderecoDestino = enderecoCompleto;
        this.cidadeDestino = dados.localidade || '';
        this.estadoDestino = dados.uf || '';
      }
    } catch (error) {
      console.error('Erro ao buscar CEP:', error);
    } finally {
      if (tipo === 'origem') {
        this.carregandoCepOrigem = false;
      } else {
        this.carregandoCepDestino = false;
      }
    }
  }
}
