import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Veiculo } from '../../models/veiculo/veiculo.model';

@Injectable({
  providedIn: 'root'
})
export class VeiculoService {
  private apiUrl = `${environment.apiUrl}/Veiculo`;

  constructor(private http: HttpClient) {}

  getMeusVeiculos(): Observable<Veiculo[]> {
    return this.http.get<Veiculo[]>(`${this.apiUrl}/meus`);
  }

  criarVeiculo(veiculo: Veiculo): Observable<Veiculo> {
    return this.http.post<Veiculo>(this.apiUrl, veiculo);
  }

  getVeiculoById(id: number): Observable<Veiculo> {
    return this.http.get<Veiculo>(`${this.apiUrl}/${id}`);
  }

  atualizarVeiculo(id: number, veiculo: Veiculo): Observable<Veiculo> {
    return this.http.put<Veiculo>(`${this.apiUrl}/${id}`, veiculo);
  }

  deletarVeiculo(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getVeiculosByTransportador(transportadorId: number): Observable<Veiculo[]> {
    return this.http.get<Veiculo[]>(`${this.apiUrl}/transportador/${transportadorId}`);
  }
}