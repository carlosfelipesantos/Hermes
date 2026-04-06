import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Usuario } from '../../models/usuario/usuario.model';
import { Frete } from '../../models/fretes/fretes.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = `${environment.apiUrl}/Admin`;

  constructor(private http: HttpClient) {}

  listarUsuarios(page: number = 1, pageSize: number = 10): Observable<{ usuarios: Usuario[], total: number }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<{ usuarios: Usuario[], total: number }>(`${this.apiUrl}/usuarios`, { params });
  }

  listarFretes(page: number = 1, pageSize: number = 10): Observable<{ fretes: Frete[], total: number }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    return this.http.get<{ fretes: Frete[], total: number }>(`${this.apiUrl}/fretes`, { params });
  }

  ativarDesativarUsuario(id: number, ativo: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/usuarios/${id}/status`, ativo);
  }
}