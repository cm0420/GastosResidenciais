namespace GastosResidenciais.Api.Enums;

/// <summary>
/// Representa a natureza de uma transação financeira: entrada (Receita) ou saída (Despesa) de dinheiro.
/// </summary>
public enum TipoTransacao
{
    /// <summary>Saída de dinheiro (gasto).</summary>
    Despesa = 0,

    /// <summary>Entrada de dinheiro (ganho).</summary>
    Receita = 1
}
