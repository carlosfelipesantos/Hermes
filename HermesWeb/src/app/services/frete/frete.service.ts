import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Frete, FreteAgendadoRequest, FreteImediatoRequest } from '../../models/fretes/fretes.model';

@Injectable({
  providedIn: 'root'
})
export class FreteService {
  private apiUrl = `${environment.apiUrl}/Frete`;

  constructor(private http: HttpClient) {}

  // Cliente
  criarFreteImediato(frete: FreteImediatoRequest): Observable<Frete> {
    return this.http.post<Frete>(`${this.apiUrl}/imediató`, frete);
  }

  criarFreteAgendado(frete: FreteAgendadoRequest): Observable<Frete> {
    return this.http.post<Frete>(`${this.apiUrl}/agendado`, frete);
  }

  getMeusFretes(): Observable<Frete[]> {
    return this.http.get<Frete[]>(`${this.apiUrl}/meus`);
  }

  // Transportador
  getMeusTransportes(): Observable<Frete[]> {
    return this.http.get<Frete[]>(`${this.apiUrl}/meus-transportes`);
  }

  getFretesDisponiveis(): Observable<Frete[]> {
    return this.http.get<Frete[]>(`${this.apiUrl}/disponiveis`);
  }

  aceitarFrete(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/aceitar`, {});
  }

  confirmarFrete(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/confirmar`, {});
  }

  rejeitarFrete(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/rejeitar`, {});
  }

  finalizarFrete(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/finalizar`, {});
  }

  getDuracaoEstimada(id: number): Observable<{ duracao: string }> {
    return this.http.get<{ duracao: string }>(`${this.apiUrl}/${id}/duracao-estimada`);
  }

  // Geral
  getFreteById(id: number): Observable<Frete> {
    return this.http.get<Frete>(`${this.apiUrl}/${id}`);
  }

  getFretesConcluidosHome(): Observable<Frete[]> {
    return this.http.get<Frete[]>(`${this.apiUrl}/concluidos/home`);
  }

  atualizarStatus(id: number, status: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, { status });
  }

  getFretesDisponiveisFiltrados(filtros: any): Observable<Frete[]> {
    let params = new HttpParams();
    Object.keys(filtros).forEach(key => {
      if (filtros[key]) {
        params = params.set(key, filtros[key]);
      }
    });
    return this.http.get<Frete[]>(`${this.apiUrl}/disponiveis/filtradopaginado`, { params });
  }
}