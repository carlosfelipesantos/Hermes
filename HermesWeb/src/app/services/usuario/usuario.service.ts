import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environment/environment';
import { Usuario } from '../../models/usuario/usuario.model';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private apiUrl = `${environment.apiUrl}/Usuario`;

  constructor(private http: HttpClient) {}

  cadastrarCliente(usuario: Usuario): Observable<any> {
    return this.http.post(this.apiUrl, { ...usuario, tipo: 'Cliente' });
  }

  cadastrarTransportador(usuario: Usuario): Observable<any> {
    return this.http.post(this.apiUrl, { ...usuario, tipo: 'Transportador' });
  }

  getMe(): Observable<Usuario> {
    return this.http.get<Usuario>(`${this.apiUrl}/me`);
  }

  uploadFoto(formData: FormData): Observable<{ fotoUrl: string }> {
    return this.http.post<{ fotoUrl: string }>(`${this.apiUrl}/usuarios/foto`, formData);
  }

  atualizarPerfil(usuario: Usuario): Observable<Usuario> {
    return this.http.put<Usuario>(`${this.apiUrl}/${usuario.id}`, usuario);
  }

  deleteConta(): Observable<any> {
    return this.http.delete(this.apiUrl);
  }
}