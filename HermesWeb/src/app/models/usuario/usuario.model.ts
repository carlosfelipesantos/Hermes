export interface Usuario {
  id?: number;
  nome: string;
  email: string;
  senha?: string;
  tipo: 'Cliente' | 'Transportador' | 'Admin';
  documento: string;
  telefone: string;
  ddd: string;
  estado: string;
  cidade: string;
  endereco: string;
  fotoPerfil?: string;
  ativo?: boolean;
}

export interface LoginRequest {
  email: string;
  senha: string;
}

export interface LoginResponse {
  token: string;
  usuario: Usuario;
}