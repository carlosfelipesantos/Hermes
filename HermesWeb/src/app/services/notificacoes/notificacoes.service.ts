    import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificacaoService {
  private apiUrl = `${environment.apiUrl}/Notificacao`;

  constructor(private http: HttpClient) {}

  getMinhasNotificacoes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/minhas`);
  }

  marcarComoLida(id: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/lida`, {});
  }
}