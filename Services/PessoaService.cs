using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Mappers;
using GastosResidenciais.Api.Repositories.Interfaces;
using GastosResidenciais.Api.Services.Interfaces;

namespace GastosResidenciais.Api.Services;

/// <inheritdoc cref="IPessoaService"/>
public class PessoaService(IPessoaRepository pessoaRepository) : IPessoaService
{
    public async Task<List<PessoaResponseDto>> ListarAsync()
    {
        var pessoas = await pessoaRepository.GetAllAsync();
        return pessoas.ToResponseDtoList();
    }

    public async Task<PessoaResponseDto> CriarAsync(PessoaCreateDto dto)
    {
        var pessoa = dto.ToEntity();

        await pessoaRepository.AddAsync(pessoa);
        await pessoaRepository.SaveChangesAsync();

        return pessoa.ToResponseDto();
    }

    public async Task DeletarAsync(Guid id)
    {
        var pessoa = await pessoaRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Pessoa com id '{id}' não foi encontrada.");

        // A remoção em cascata das transações é garantida pela configuração
        // OnDelete(DeleteBehavior.Cascade) feita no AppDbContext.
        pessoaRepository.Remove(pessoa);
        await pessoaRepository.SaveChangesAsync();
    }
}
