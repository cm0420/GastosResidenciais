namespace GastosResidenciais.Api.Dtos;

/// <summary>Totais financeiros consolidados de uma única pessoa.</summary>
public class TotaisPessoaDto
{
    public Guid PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}

/// <summary>Totais financeiros consolidados de todas as pessoas cadastradas, incluindo o total geral.</summary>
public class TotaisGeraisDto
{
    public List<TotaisPessoaDto> Pessoas { get; set; } = new();
    public decimal TotalReceitasGeral { get; set; }
    public decimal TotalDespesasGeral { get; set; }
    public decimal SaldoGeral { get; set; }
}
