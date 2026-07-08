using GastosResidenciais.Api.Dtos;

namespace GastosResidenciais.Api.Services.Interfaces;

/// <summary>Consultas de totais consolidados (receitas, despesas e saldo) por pessoa e geral.</summary>
public interface IRelatorioService
{
    Task<TotaisGeraisDto> ObterTotaisAsync();
}
