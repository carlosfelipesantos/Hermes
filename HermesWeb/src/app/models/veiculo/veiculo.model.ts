export interface Veiculo {
  id?: number;
  transportadorId?: number;
  tipoVeiculo: 'Moto' | 'Carro' | 'Van' | 'Caminhao' | 'Carretinha';
  marca: string;
  modelo: string;
  placa: string;
  capacidade: string;
}