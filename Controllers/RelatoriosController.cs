using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GastosResidenciais.Api.Controllers;

/// <summary>Endpoint de consulta dos totais consolidados de receitas, despesas e saldo.</summary>
[ApiController]
[Route("api/relatorios")]
[Produces("application/json")]
public class RelatoriosController(IRelatorioService relatorioService) : ControllerBase
{
    /// <summary>
    /// Retorna, para cada pessoa cadastrada, o total de receitas, despesas e saldo,
    /// além do total geral consolidando todas as pessoas.
    /// </summary>
    [HttpGet("totais")]
    [ProducesResponseType(typeof(TotaisGeraisDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TotaisGeraisDto>> ObterTotais()
    {
        var totais = await relatorioService.ObterTotaisAsync();
        return Ok(totais);
    }
}
