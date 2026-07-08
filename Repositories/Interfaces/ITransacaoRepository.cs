using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Repositories.Interfaces;

/// <summary>Contrato de acesso a dados para a entidade Transacao.</summary>
public interface ITransacaoRepository
{
    Task<List<Transacao>> GetAllAsync();
    Task<List<Transacao>> GetByPessoaIdAsync(Guid pessoaId);
    Task AddAsync(Transacao transacao);
    Task SaveChangesAsync();
}
