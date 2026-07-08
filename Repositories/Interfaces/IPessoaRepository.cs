using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories.Interfaces;

/// <summary>Contrato de acesso a dados para a entidade Pessoa.</summary>
public interface IPessoaRepository
{
    Task<List<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByIdAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task AddAsync(Pessoa pessoa);
    void Remove(Pessoa pessoa);
    Task SaveChangesAsync();
}
