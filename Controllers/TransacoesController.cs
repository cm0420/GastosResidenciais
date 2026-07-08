using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoints para gerenciamento do cadastro de transações financeiras.</summary>
[ApiController]
[Route("api/transacoes")]
[Produces("application/json")]
public class TransacoesController(ITransacaoService transacaoService) : ControllerBase
{
    /// <summary>Lista todas as transações cadastradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TransacaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TransacaoResponseDto>>> Listar()
    {
        var transacoes = await transacaoService.ListarAsync();
        return Ok(transacoes);
    }

    /// <summary>
    /// Cadastra uma nova transação. Caso a pessoa seja menor de idade, apenas despesas são permitidas.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransacaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<TransacaoResponseDto>> Criar([FromBody] TransacaoCreateDto dto)
    {
        var transacaoCriada = await transacaoService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = transacaoCriada.Id }, transacaoCriada);
    }
}
