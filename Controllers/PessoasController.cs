using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoints para gerenciamento do cadastro de pessoas.</summary>
[ApiController]
[Route("api/pessoas")]
[Produces("application/json")]
public class PessoasController(IPessoaService pessoaService) : ControllerBase
{
    /// <summary>Lista todas as pessoas cadastradas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<PessoaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PessoaResponseDto>>> Listar()
    {
        var pessoas = await pessoaService.ListarAsync();
        return Ok(pessoas);
    }

    /// <summary>Cadastra uma nova pessoa.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PessoaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PessoaResponseDto>> Criar([FromBody] PessoaCreateDto dto)
    {
        var pessoaCriada = await pessoaService.CriarAsync(dto);
        return CreatedAtAction(nameof(Listar), new { id = pessoaCriada.Id }, pessoaCriada);
    }

    /// <summary>
    /// Remove uma pessoa pelo id. Todas as transações vinculadas a ela também são removidas.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        await pessoaService.DeletarAsync(id);
        return NoContent();
    }
}
