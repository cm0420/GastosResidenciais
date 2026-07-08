using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Repositories;

/// <inheritdoc cref="ITransacaoRepository"/>
public class TransacaoRepository(AppDbContext context) : ITransacaoRepository
{
    public async Task<List<Transacao>> GetAllAsync()
    {
        return await context.Transacoes
            .Include(t => t.Pessoa)
            .AsNoTracking()
            .OrderByDescending(t => t.Id)
            .ToListAsync();
    }

    public async Task<List<Transacao>> GetByPessoaIdAsync(Guid pessoaId)
    {
        return await context.Transacoes
            .AsNoTracking()
            .Where(t => t.PessoaId == pessoaId)
            .ToListAsync();
    }

    public async Task AddAsync(Transacao transacao)
    {
        await context.Transacoes.AddAsync(transacao);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
