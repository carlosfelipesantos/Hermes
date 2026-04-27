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
  alertas: { tipo: 'success' | 'error' | 'info' | 'warning'; mensagem: string }[] = [];

  constructor(
    private freteService: FreteService,
    private router: Router
  ) {}

  mostrarAlerta(tipo: 'success' | 'error' | 'info' | 'warning', mensagem: string): void {
    this.alertas.push({ tipo, mensagem });
    setTimeout(() => {
      this.alertas = this.alertas.filter(a => a.mensagem !== mensagem);
    }, 5000);
  }

  fecharAlerta(index: number): void {
    this.alertas.splice(index, 1);
  }

  formatarCep(tipo: 'origem' | 'destino'): void {
    const campo = tipo === 'origem' ? 'cepOrigem' : 'cepDestino';
    let valor = (this[campo] as string).replace(/\D/g, '');
    
    if (valor.length > 8) {
      valor = valor.slice(0, 8);
    }
    
    if (valor.length > 5) {
      valor = valor.slice(0, 5) + '-' + valor.slice(5, 8);
    }
    
    (this[campo] as string) = valor;

    // Busca automática quando completar 8 dígitos
    const cepNumeros = valor.replace(/\D/g, '');
    if (cepNumeros.length === 8) {
      this.buscarCep(tipo);
    }
  }

  async buscarCep(tipo: 'origem' | 'destino'): Promise<void> {
    const cep = (tipo === 'origem' ? this.cepOrigem : this.cepDestino).replace(/\D/g, '');
    
    if (cep.length !== 8) {
      this.mostrarAlerta('warning', 'CEP deve ter 8 dígitos');
      return;
    }

    // Seta loading
    if (tipo === 'origem') {
      this.carregandoCepOrigem = true;
    } else {
      this.carregandoCepDestino = true;
    }

    try {
      const response = await fetch(`https://viacep.com.br/ws/${cep}/json/`);
      
      if (!response.ok) {
        throw new Error('Erro na requisição');
      }

      const data = await response.json();

      if (data.erro) {
        this.mostrarAlerta('error', `CEP ${cep} não encontrado`);
        this.limparCamposCep(tipo);
        return;
      }

      // Preenche os campos
      if (tipo === 'origem') {
        this.enderecoOrigem = data.logradouro || '';
        this.bairroOrigem = data.bairro || '';
        this.cidadeOrigem = data.localidade || '';
        this.estadoOrigem = data.uf || '';
        this.dddOrigem = data.ddd || '';
        
        this.mostrarAlerta('success', 'Endereço de origem preenchido automaticamente!');
      } else {
        this.enderecoDestino = data.logradouro || '';
        this.bairroDestino = data.bairro || '';
        this.cidadeDestino = data.localidade || '';
        this.estadoDestino = data.uf || '';
        
        this.mostrarAlerta('success', 'Endereço de destino preenchido automaticamente!');
      }

      // Alerta se faltar alguma informação
      if (!data.logradouro || !data.bairro) {
        this.mostrarAlerta('warning', 'Alguns campos precisam ser preenchidos manualmente');
      }

    } catch (error) {
      console.error('Erro ao buscar CEP:', error);
      this.mostrarAlerta('error', 'Erro ao consultar CEP. Tente novamente.');
      this.limparCamposCep(tipo);
    } finally {
      if (tipo === 'origem') {
        this.carregandoCepOrigem = false;
      } else {
        this.carregandoCepDestino = false;
      }
    }
  }

  limparCamposCep(tipo: 'origem' | 'destino'): void {
    if (tipo === 'origem') {
      this.enderecoOrigem = '';
      this.bairroOrigem = '';
      this.cidadeOrigem = '';
      this.estadoOrigem = '';
      this.dddOrigem = '';
    } else {
      this.enderecoDestino = '';
      this.bairroDestino = '';
      this.cidadeDestino = '';
      this.estadoDestino = '';
    }
  }

  validarFormulario(): boolean {
    const erros: string[] = [];

    // Validação de origem
    if (!this.enderecoOrigem?.trim()) {
      erros.push('Endereço de origem é obrigatório');
    }
    if (!this.bairroOrigem?.trim()) {
      erros.push('Bairro de origem é obrigatório');
    }
    if (!this.cidadeOrigem?.trim()) {
      erros.push('Cidade de origem é obrigatória');
    }
    if (!this.estadoOrigem?.trim()) {
      erros.push('Estado de origem é obrigatório');
    }
    if (this.estadoOrigem?.trim().length !== 2) {
      erros.push('Estado de origem deve ter 2 caracteres (UF)');
    }

    // Validação de destino
    if (!this.enderecoDestino?.trim()) {
      erros.push('Endereço de destino é obrigatório');
    }
    if (!this.bairroDestino?.trim()) {
      erros.push('Bairro de destino é obrigatório');
    }
    if (!this.cidadeDestino?.trim()) {
      erros.push('Cidade de destino é obrigatória');
    }
    if (!this.estadoDestino?.trim()) {
      erros.push('Estado de destino é obrigatório');
    }
    if (this.estadoDestino?.trim().length !== 2) {
      erros.push('Estado de destino deve ter 2 caracteres (UF)');
    }

    // Validação da carga
    if (!this.tipoCarga) {
      erros.push('Selecione o tipo de carga');
    }

    const valorLimpo = this.valor.replace(/[^\d,]/g, '').replace(',', '.');
    if (!this.valor || isNaN(parseFloat(valorLimpo)) || parseFloat(valorLimpo) <= 0) {
      erros.push('Informe um valor válido para o frete');
    }

    if (!this.descricaoCarga?.trim()) {
      erros.push('Descrição da carga é obrigatória');
    }

    // Exibe os erros
    if (erros.length > 0) {
      erros.forEach(erro => this.mostrarAlerta('error', erro));
      return false;
    }

    return true;
  }

  solicitarFrete(): void {
    this.alertas = [];
    this.erro = '';

    if (!this.validarFormulario()) {
      return;
    }

    this.enviando = true;

    const valorNumerico = parseFloat(
      this.valor.replace('R$', '')
                .replace(/\./g, '')
                .replace(',', '.')
                .trim()
    );

    const payload = {
      tipoCarga: this.tipoCarga as 'Pequena' | 'Media' | 'Grande',
      status: 'Pendente' as const,
      urgente: this.urgente,

      dddOrigem: this.dddOrigem,
      cidadeOrigem: this.cidadeOrigem.trim(),
      bairroOrigem: this.bairroOrigem.trim(),
      estadoOrigem: this.estadoOrigem.trim().toUpperCase(),
      enderecoOrigem: this.enderecoOrigem.trim(),
      latitudeOrigem: 0,
      longitudeOrigem: 0,

      cidadeDestino: this.cidadeDestino.trim(),
      bairroDestino: this.bairroDestino.trim(),
      estadoDestino: this.estadoDestino.trim().toUpperCase(),
      enderecoDestino: this.enderecoDestino.trim(),
      latitudeDestino: 0,
      longitudeDestino: 0,

      descricaoCarga: this.descricaoCarga.trim(),
      valor: valorNumerico,

      sitioOrigem: this.sitioOrigem,
      sitioDestino: this.sitioDestino,

      transportadorId: 0
    };

    console.log('Payload enviado:', payload); // Debug

    this.freteService.criarFreteImediato(payload).subscribe({
      next: (response) => {
        console.log('Resposta sucesso:', response); // Debug
        this.enviando = false;
        this.sucesso = true;
        this.mostrarAlerta('success', '✅ Frete solicitado com sucesso! Redirecionando...');
        setTimeout(() => this.router.navigate(['/dashboard']), 3000);
      },
      error: (err) => {
        console.error('Erro completo:', err); // Debug
        this.enviando = false;
        
        const mensagemErro = err?.error?.mensagem || 
                             err?.error?.message || 
                             err?.message || 
                             'Erro ao solicitar frete. Tente novamente.';
        
        this.mostrarAlerta('error', `❌ ${mensagemErro}`);
      }
    });
  }
}