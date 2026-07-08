using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Enums;
using GastosResidenciais.Api.Exceptions;
using GastosResidenciais.Api.Mappers;
using GastosResidenciais.Api.Repositories.Interfaces;
using GastosResidenciais.Api.Services.Interfaces;

namespace GastosResidenciais.Api.Services;

/// <inheritdoc cref="ITransacaoService"/>
public class TransacaoService(
    ITransacaoRepository transacaoRepository,
    IPessoaRepository pessoaRepository) : ITransacaoService
{
    public async Task<List<TransacaoResponseDto>> ListarAsync()
    {
        var transacoes = await transacaoRepository.GetAllAsync();
        return transacoes.ToResponseDtoList();
    }

    public async Task<TransacaoResponseDto> CriarAsync(TransacaoCreateDto dto)
    {
        var pessoa = await pessoaRepository.GetByIdAsync(dto.PessoaId)
            ?? throw new NotFoundException($"Pessoa com id '{dto.PessoaId}' não foi encontrada.");

        // Regra de negócio: pessoas menores de 18 anos só podem ter despesas cadastradas.
        if (pessoa.EhMenorDeIdade && dto.Tipo == TipoTransacao.Receita)
        {
            throw new BusinessRuleException(
                $"'{pessoa.Nome}' é menor de idade e, por isso, apenas despesas podem ser cadastradas para essa pessoa.");
        }

        var transacao = dto.ToEntity();

        await transacaoRepository.AddAsync(transacao);
        await transacaoRepository.SaveChangesAsync();

        transacao.Pessoa = pessoa;
        return transacao.ToResponseDto();
    }
}
