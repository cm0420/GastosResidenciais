using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Mappers;

/// <summary>Conversões entre a entidade Transacao e seus DTOs.</summary>
public static class TransacaoMapper
{
    public static Transacao ToEntity(this TransacaoCreateDto dto)
    {
        return new Transacao
        {
            Descricao = dto.Descricao,
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };
    }

    public static TransacaoResponseDto ToResponseDto(this Transacao entity)
    {
        return new TransacaoResponseDto
        {
            Id = entity.Id,
            Descricao = entity.Descricao,
            Valor = entity.Valor,
            Tipo = entity.Tipo,
            PessoaId = entity.PessoaId,
            PessoaNome = entity.Pessoa?.Nome
        };
    }

    public static List<TransacaoResponseDto> ToResponseDtoList(this IEnumerable<Transacao> entities)
    {
        return entities.Select(ToResponseDto).ToList();
    }
}
