export interface Disponibilidade {
  id?: number;
  transportadorId?: number;
  diaSemana: 'Domingo' | 'Segunda-feira' | 'Terça-feira' | 'Quarta-feira' | 'Quinta-feira' | 'Sexta-feira' | 'Sábado';
  horaInicio: string;
  horaFim: string;
}