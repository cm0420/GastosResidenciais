using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Mappers;

/// <summary>
/// Conversões entre a entidade Pessoa e seus DTOs.
/// Implementado como extension methods simples para manter baixo acoplamento sem depender de
/// bibliotecas externas de mapeamento (AutoMapper, Mapster, etc.).
/// </summary>
public static class PessoaMapper
{
    public static Pessoa ToEntity(this PessoaCreateDto dto)
    {
        return new Pessoa
        {
            Nome = dto.Nome,
            Idade = dto.Idade
        };
    }

    public static PessoaResponseDto ToResponseDto(this Pessoa entity)
    {
        return new PessoaResponseDto
        {
            Id = entity.Id,
            Nome = entity.Nome,
            Idade = entity.Idade
        };
    }

    public static List<PessoaResponseDto> ToResponseDtoList(this IEnumerable<Pessoa> entities)
    {
        return entities.Select(ToResponseDto).ToList();
    }
}
