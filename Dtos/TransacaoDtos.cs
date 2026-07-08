using System.ComponentModel.DataAnnotations;
using GastosResidenciais.Api.Enums;

namespace GastosResidenciais.Api.Dtos;

/// <summary>Dados necessários para cadastrar uma nova transação.</summary>
public class TransacaoCreateDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O tipo da transação é obrigatório.")]
    public TipoTransacao Tipo { get; set; }

    [Required(ErrorMessage = "A pessoa é obrigatória.")]
    public Guid PessoaId { get; set; }
}

/// <summary>Representação de uma transação retornada pela API.</summary>
public class TransacaoResponseDto
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public Guid PessoaId { get; set; }
    public string? PessoaNome { get; set; }
}
