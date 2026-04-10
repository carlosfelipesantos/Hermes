import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Avaliacao } from '../../models/avaliacao/avaliacao.model';

@Injectable({
  providedIn: 'root'
})
export class AvaliacaoService {
  private apiUrl = `${environment.apiUrl}/Avaliacao`;

  constructor(private http: HttpClient) {}

  getAvaliacoesRecebidas(idUsuario: number): Observable<Avaliacao[]> {
    return this.http.get<Avaliacao[]>(`${this.apiUrl}/recebidas`);
  }
    
  getMinhasAvaliacoes(idUsuario: number): Observable<Avaliacao[]> {
    return this.http.get<Avaliacao[]>(`${this.apiUrl}/minhas`);
  }
}