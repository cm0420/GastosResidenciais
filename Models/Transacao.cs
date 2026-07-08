using GastosResidenciais.Api.Enums;

namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) associada a uma pessoa.
/// </summary>
public class Transacao
{
    /// <summary>Identificador único, gerado automaticamente pelo banco de dados.</summary>
    public Guid Id { get; set; }

    /// <summary>Descrição livre da transação (ex: "Supermercado", "Salário").</summary>
    public string Descricao { get; set; } = string.Empty;

    /// <summary>Valor monetário da transação. Sempre positivo; o sinal é definido pelo campo Tipo.</summary>
    public decimal Valor { get; set; }

    /// <summary>Define se a transação é uma Receita ou uma Despesa.</summary>
    public TipoTransacao Tipo { get; set; }

    /// <summary>Chave estrangeira para a pessoa dona da transação.</summary>
    public Guid PessoaId { get; set; }

    /// <summary>Navegação para a pessoa dona da transação.</summary>
    public Pessoa? Pessoa { get; set; }
}
