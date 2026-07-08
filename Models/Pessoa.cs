namespace GastosResidenciais.Api.Models;

/// <summary>
/// Representa uma pessoa cadastrada no sistema, dona de zero ou mais transações financeiras.
/// </summary>
public class Pessoa
{
    /// <summary>Identificador único, gerado automaticamente pelo banco de dados.</summary>
    public Guid Id { get; set; }

    /// <summary>Nome completo da pessoa.</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Idade da pessoa. Usada para aplicar a regra de negócio de menores de idade.</summary>
    public int Idade { get; set; }

    /// <summary>
    /// Transações associadas a esta pessoa.
    /// Ao deletar a pessoa, todas as transações relacionadas são removidas em cascata (configurado no AppDbContext).
    /// </summary>
    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();

    /// <summary>Indica se a pessoa é menor de idade (regra de negócio: menor de 18 anos).</summary>
    public bool EhMenorDeIdade => Idade < 18;
}
