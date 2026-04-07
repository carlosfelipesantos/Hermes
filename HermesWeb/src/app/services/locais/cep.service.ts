import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EnderecoViaCep {
  cep: string;
  logradouro: string;
  complemento: string;
  bairro: string;
  localidade: string; // cidade
  uf: string;         // estado
  erro?: boolean;
}

@Injectable({ providedIn: 'root' })
export class CepService {
  constructor(private http: HttpClient) {}

  consultarCep(cep: string): Observable<EnderecoViaCep> {
    const cepLimpo = cep.replace(/\D/g, '');
    return this.http.get<EnderecoViaCep>(`https://viacep.com.br/ws/${cepLimpo}/json/`);
  }
}