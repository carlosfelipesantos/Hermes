using Hermes.Data;
using Hermes.Entities;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class VeiculoService : IVeiculoService
    {
        private readonly HermesBD _context;

        public VeiculoService(HermesBD context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Veiculo>> ListarPorTransportador(int transportadorId)
        {
            return await _context.Veiculos
                .Where(v => v.TransportadorId == transportadorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Veiculo>> Listar()
        {
            return await _context.Veiculos
               .Include(v => v.Transportador)
               .ToListAsync();
        }

        public async Task<Veiculo> Criar(Veiculo veiculo)
        {
            // Validar se o transportador existe e está ativo
            var transportador = await _context.Transportadores
                .FirstOrDefaultAsync(t => t.Id == veiculo.TransportadorId && t.Ativo);

            if (transportador == null)
                throw new NotFoundException("Transportador não encontrado ou inativo");

            // Validar se a placa já existe (independente do transportador)
            var placaExiste = await _context.Veiculos
                .AnyAsync(v => v.Placa == veiculo.Placa);
            if (placaExiste)
                throw new BusinessException("Esta placa já está cadastrada no sistema");

            veiculo.DataCadastro = TimeHelper.Now;
            veiculo.Disponivel = true; // por padrão, veículo está disponível

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();

            return veiculo;
        }

        public async Task<Veiculo> BuscarPorId(int id)
        {
            var veiculo = await _context.Veiculos
                .Include(v => v.Transportador)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (veiculo == null)
                throw new NotFoundException($"Veículo {id} não encontrado");

            return veiculo;
        }

        public async Task Atualizar(Veiculo veiculo)
        {
            var veiculoExistente = await _context.Veiculos.FindAsync(veiculo.Id);
            if (veiculoExistente == null)
                throw new NotFoundException($"Veículo {veiculo.Id} não encontrado");

            _context.Veiculos.Update(veiculo);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Deletar(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
                throw new NotFoundException($"Veículo {id} não encontrado");

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}