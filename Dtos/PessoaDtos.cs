using System.ComponentModel.DataAnnotations;

namespace GastosResidenciais.Api.Dtos;

/// <summary>Dados necessários para cadastrar uma nova pessoa.</summary>
public class PessoaCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0, 150, ErrorMessage = "A idade deve estar entre 0 e 150 anos.")]
    public int Idade { get; set; }
}

/// <summary>Representação de uma pessoa retornada pela API.</summary>
public class PessoaResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
}
