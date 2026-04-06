export interface Avaliacao {
  id?: number;
  freteId: number;
  transportadorId: number;
  clienteId?: number;
  nota: number;
  comentario: string;
  dataAvaliacao?: string;
}