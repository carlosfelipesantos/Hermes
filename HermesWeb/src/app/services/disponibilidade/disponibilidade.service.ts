import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Disponibilidade } from '../../models/disponibilidade/disponibilidade.model';

@Injectable({
  providedIn: 'root'
})
export class DisponibilidadeService {
  private apiUrl = `${environment.apiUrl}/Disponibilidade`;

  constructor(private http: HttpClient) {}

  getMinhasDisponibilidades(): Observable<Disponibilidade[]> {
    return this.http.get<Disponibilidade[]>(`${this.apiUrl}/meus`);
  }

  criarDisponibilidade(disponibilidade: Disponibilidade): Observable<Disponibilidade> {
    return this.http.post<Disponibilidade>(this.apiUrl, disponibilidade);
  }

  atualizarDisponibilidade(id: number, disponibilidade: Disponibilidade): Observable<Disponibilidade> {
    return this.http.put<Disponibilidade>(`${this.apiUrl}/${id}`, disponibilidade);
  }

  deletarDisponibilidade(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getIntervalosLivres(id: number, data: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}/intervalos?data=${data}`);
  }
}