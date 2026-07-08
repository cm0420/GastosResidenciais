using GastosResidenciais.Api.Dtos;

namespace GastosResidenciais.Api.Services.Interfaces;

/// <summary>Regras de negócio relacionadas ao cadastro de transações.</summary>
public interface ITransacaoService
{
    Task<List<TransacaoResponseDto>> ListarAsync();
    Task<TransacaoResponseDto> CriarAsync(TransacaoCreateDto dto);
}
