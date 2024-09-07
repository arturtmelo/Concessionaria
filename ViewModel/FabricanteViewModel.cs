using System.ComponentModel.DataAnnotations;

public class FabricanteViewModel
{
    public int FabricanteID { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; }

    [MaxLength(50, ErrorMessage = "País de Origem deve ter no máximo 50 caracteres.")]
    public string PaisOrigem { get; set; }

    [Required(ErrorMessage = "Ano de Fundação é obrigatório.")]
    [Range(1810, int.MaxValue, ErrorMessage = "O ano deve ser entre 1810 e o atual.")]
    public int AnoFundacao { get; set; }

    [Url(ErrorMessage = "Site deve conter uma URL válida.")]
    public string Site { get; set; }
}
