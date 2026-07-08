using GastosResidenciais.Api.Dtos;

namespace GastosResidenciais.Api.Services.Interfaces;

/// <summary>Regras de negócio relacionadas ao cadastro de pessoas.</summary>
public interface IPessoaService
{
    Task<List<PessoaResponseDto>> ListarAsync();
    Task<PessoaResponseDto> CriarAsync(PessoaCreateDto dto);
    Task DeletarAsync(Guid id);
}
