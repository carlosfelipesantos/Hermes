import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../../../../services/usuario/usuario.service';
import { CepService } from '../../../../services/locais/cep.service';

@Component({
  selector: 'app-cadastro',
  standalone: false,
  templateUrl: './cadastro.html',
  styleUrls: ['./cadastro.css']
})
export class Cadastro implements OnInit {
  cadastroForm: FormGroup;
  loading = false;
  errorMessage = '';
  successMessage = '';
  tipoSelecionado: 'Cliente' | 'Transportador' = 'Cliente';

  constructor(
    private fb: FormBuilder,
    private usuarioService: UsuarioService,
    private cepService: CepService,
    private router: Router
  ) {
    this.cadastroForm = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarSenha: ['', Validators.required],
      tipo: ['Cliente', Validators.required],
      documento: ['', Validators.required],
      dataNascimento: ['', Validators.required],
      telefone: ['', [Validators.required, Validators.pattern(/^\d{8,9}$/)]],
      ddd: ['', [Validators.required, Validators.pattern(/^\d{2}$/)]],
      cep: ['', [Validators.required, Validators.pattern(/^\d{5}-?\d{3}$/)]],
      logradouro: ['', Validators.required],
      numero: ['', Validators.required],
      complemento: [''],
      bairro: ['', Validators.required],
      cidade: ['', Validators.required],
      estado: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }

  ngOnInit(): void {
    // Consulta CEP quando o campo perder o foco (blur)
    const cepControl = this.cadastroForm.get('cep');
    cepControl?.valueChanges.subscribe(cep => {
      const cepLimpo = cep?.replace(/\D/g, '');
      if (cepLimpo && cepLimpo.length === 8) {
        this.buscarEnderecoPorCep(cepLimpo);
      }
    });
  }

  // Validador de senhas iguais
  passwordMatchValidator(group: FormGroup): any {
    const senha = group.get('senha')?.value;
    const confirmar = group.get('confirmarSenha')?.value;
    return senha === confirmar ? null : { mismatch: true };
  }

  // Máscara para CPF ou CNPJ conforme o tipo
  aplicarMascaraCpfCnpj(event: any): void {
    let valor = event.target.value.replace(/\D/g, '');
    if (this.tipoSelecionado === 'Cliente') {
      // CPF: 000.000.000-00
      if (valor.length <= 11) {
        valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
        valor = valor.replace(/(\d{3})(\d)/, '$1.$2');
        valor = valor.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
      }
    } else {
      // CNPJ: 00.000.000/0000-00
      if (valor.length <= 14) {
        valor = valor.replace(/^(\d{2})(\d)/, '$1.$2');
        valor = valor.replace(/^(\d{2})\.(\d{3})(\d)/, '$1.$2.$3');
        valor = valor.replace(/\.(\d{3})(\d)/, '.$1/$2');
        valor = valor.replace(/(\d{4})(\d)/, '$1-$2');
      }
    }
    this.cadastroForm.get('documento')?.setValue(valor, { emitEvent: false });
  }

  aplicarMascaraTelefone(event: any): void {
    let valor = event.target.value.replace(/\D/g, '');
    if (valor.length >= 7) {
      if (valor.length === 10) {
        valor = valor.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
      } else if (valor.length === 11) {
        valor = valor.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
      } 
    }
    this.cadastroForm.get('telefone')?.setValue(valor, { emitEvent: false });
  }

  aplicarMascaraCep(event: any): void {
    let valor = event.target.value.replace(/\D/g, '');
    if (valor.length > 5) {
      valor = valor.replace(/^(\d{5})(\d)/, '$1-$2');
    }
    this.cadastroForm.get('cep')?.setValue(valor, { emitEvent: false });
  }

  buscarEnderecoPorCep(cep: string): void {
    this.cepService.consultarCep(cep).subscribe({
      next: (endereco: any) => {
        if (endereco.erro) {
          this.errorMessage = 'CEP não encontrado.';
          return;
        }
        this.cadastroForm.patchValue({
          logradouro: endereco.logradouro || '',
          bairro: endereco.bairro || '',
          cidade: endereco.localidade || '',
          estado: endereco.uf || '',
          complemento: endereco.complemento || ''
        });
        this.errorMessage = '';
      },
      error: () => {
        this.errorMessage = 'Erro ao buscar CEP. Tente novamente.';
      }
    });
  }

  onTipoChange(tipo: string): void {
    this.tipoSelecionado = tipo as 'Cliente' | 'Transportador';
    this.cadastroForm.patchValue({ tipo, documento: '' });
  }

  onSubmit(): void {
    console.log('Submit acionado');
    console.log('Form válido?', this.cadastroForm.valid);
    console.log('Erros do form:', this.cadastroForm.errors);
    console.log('Valores:', this.cadastroForm.value);
    console.log('Botão submit clicado');
    if (this.cadastroForm.invalid) {
      console.log('Formulário inválido', this.cadastroForm.errors);
      Object.keys(this.cadastroForm.controls).forEach(key => {
        const control = this.cadastroForm.get(key);
        if (control?.invalid) {
          console.log(`Campo inválido: ${key}`, control.errors);
        }
        control?.markAsTouched();
      });
      this.errorMessage = 'Preencha todos os campos corretamente.';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const formValue = this.cadastroForm.value;
    const { confirmarSenha, ...dadosEnvio } = formValue;

    const documentoLimpo = dadosEnvio.documento.replace(/\D/g, '');
    const telefoneLimpo = dadosEnvio.telefone.replace(/\D/g, '');
    const cepLimpo = dadosEnvio.cep.replace(/\D/g, '');

    const enderecoCompleto = `${dadosEnvio.logradouro}, ${dadosEnvio.numero}${dadosEnvio.complemento ? ', ' + dadosEnvio.complemento : ''} - ${dadosEnvio.bairro}`;

    const request = {
      nome: dadosEnvio.nome,
      email: dadosEnvio.email,
      senha: dadosEnvio.senha,
      tipo: dadosEnvio.tipo,
      documento: documentoLimpo,
      dataNascimento: dadosEnvio.dataNascimento,
      telefone: telefoneLimpo,
      ddd: dadosEnvio.ddd,
      estado: dadosEnvio.estado,
      cidade: dadosEnvio.cidade,
      endereco: enderecoCompleto,
      cep: cepLimpo,
      fotoPerfil: ''
    };

    console.log('Enviando requisição:', request);

    const cadastroObservable = dadosEnvio.tipo === 'Cliente'
      ? this.usuarioService.cadastrarCliente(request)
      : this.usuarioService.cadastrarTransportador(request);

    cadastroObservable.subscribe({
      next: (response) => {
        console.log('Resposta do servidor:', response);
        this.successMessage = 'Cadastro realizado com sucesso! Redirecionando...';
        setTimeout(() => this.router.navigate(['/auth/login']), 2000);
      },
      error: (err) => {
        console.error('Erro no cadastro:', err);
        this.errorMessage = err.error?.message || 'Erro ao cadastrar. Tente novamente.';
        this.loading = false;
      }
    });
  }
}