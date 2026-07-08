using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Models;
using GastosResidenciais.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GastosResidenciais.Api.Repositories;

/// <inheritdoc cref="IPessoaRepository"/>
public class PessoaRepository(AppDbContext context) : IPessoaRepository
{
    public async Task<List<Pessoa>> GetAllAsync()
    {
        return await context.Pessoas
            .Include(p => p.Transacoes)
            .AsNoTracking()
            .OrderBy(p => p.Nome)
            .ToListAsync();
    }

    public async Task<Pessoa?> GetByIdAsync(Guid id)
    {
        return await context.Pessoas
            .Include(p => p.Transacoes)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Pessoas.AnyAsync(p => p.Id == id);
    }

    public async Task AddAsync(Pessoa pessoa)
    {
        await context.Pessoas.AddAsync(pessoa);
    }

    public void Remove(Pessoa pessoa)
    {
        context.Pessoas.Remove(pessoa);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}
