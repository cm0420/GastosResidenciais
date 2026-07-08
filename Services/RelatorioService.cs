using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Enums;
using GastosResidenciais.Api.Repositories.Interfaces;
using GastosResidenciais.Api.Services.Interfaces;

namespace GastosResidenciais.Api.Services;

/// <inheritdoc cref="IRelatorioService"/>
public class RelatorioService(IPessoaRepository pessoaRepository) : IRelatorioService
{
    public async Task<TotaisGeraisDto> ObterTotaisAsync()
    {
        var pessoas = await pessoaRepository.GetAllAsync();

        var totaisPorPessoa = pessoas
            .Select(pessoa =>
            {
                var totalReceitas = pessoa.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = pessoa.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                return new TotaisPessoaDto
                {
                    PessoaId = pessoa.Id,
                    Nome = pessoa.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                };
            })
            .ToList();

        return new TotaisGeraisDto
        {
            Pessoas = totaisPorPessoa,
            TotalReceitasGeral = totaisPorPessoa.Sum(p => p.TotalReceitas),
            TotalDespesasGeral = totaisPorPessoa.Sum(p => p.TotalDespesas),
            SaldoGeral = totaisPorPessoa.Sum(p => p.Saldo)
        };
    }
}
