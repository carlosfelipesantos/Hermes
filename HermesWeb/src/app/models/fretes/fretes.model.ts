export interface Frete {
  id?: number;
  clienteId?: number;
  transportadorId?: number;
  tipoCarga: 'Pequena' | 'Media' | 'Grande';
  status: 'Pendente' | 'Aceito' | 'Agendado' | 'EmTransito' | 'Concluido' | 'Cancelado';
  urgente: boolean;
  dddOrigem: string;
  cidadeOrigem: string;
  bairroOrigem: string;
  estadoOrigem: string;
  latitudeOrigem: number;
  longitudeOrigem: number;
  cidadeDestino: string;
  bairroDestino: string;
  estadoDestino: string;
  latitudeDestino: number;
  longitudeDestino: number;
  descricaoCarga: string;
  valor: number;
  sitioOrigem: boolean;
  sitioDestino: boolean;
  descricaoOrigem?: string;
  descricaoDestino?: string;
  dataHoraInicio?: string;
  dataHoraConclusao?: string;
  duracaoEstimada?: string;
}

export interface FreteImediatoRequest extends Frete {
  // Sem transportadorId e dataHoraInicio
}

export interface FreteAgendadoRequest extends Frete {
  transportadorId: number;
  dataHoraInicio: string;
}