using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcessionariaMVC.Models
{
    public class Fabricante
    {
        public int FabricanteID { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
        [Index(IsUnique = true)]
        public string Nome { get; set; }

        [MaxLength(50, ErrorMessage = "País de Origem deve ter no máximo 50 caracteres.")]
        public string PaisOrigem { get; set; }

        [Required(ErrorMessage = "Ano de Fundação é obrigatório.")]
        [Range(1810, int.MaxValue, ErrorMessage = "O ano deve ser entre 1810 e o atual.")]
        [CustomYearValidation(ErrorMessage = "O ano de fundação não pode ser no futuro.")]
        public int AnoFundacao { get; set; }

        [Url(ErrorMessage = "Site deve conter uma URL válida.")]
        public string Site { get; set; }

        public bool BitAtivo { get; set; } = false;

        public ICollection<Veiculo> Veiculos { get; set; }
    }

}
