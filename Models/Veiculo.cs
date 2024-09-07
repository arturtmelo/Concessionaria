using System;
using System.ComponentModel.DataAnnotations;

namespace ConcessionariaMVC.Models
{
    public class Veiculo
    {
        public int VeiculoID { get; set; }

        [Required(ErrorMessage = "O campo Modelo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Modelo deve ter no máximo 100 caracteres.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "O campo Ano de Fabricação é obrigatório.")]
        [Range(1886, int.MaxValue, ErrorMessage = "O ano deve ser entre 1886 e o atual.")]
        [CustomYearValidation(ErrorMessage = "O ano de fabricação não pode ser no futuro.")]
        public int AnoFabricacao { get; set; }

        [Required(ErrorMessage = "O campo Preço é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo.")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo Fabricante é obrigatório.")]
        public int FabricanteID { get; set; }
        public Fabricante Fabricante { get; set; }

        [Required(ErrorMessage = "O campo Tipo de Veículo é obrigatório.")]
        public EnumTipoVeiculo TipoVeiculo { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres.")]
        public string Descricao { get; set; }

        public bool BitAtivo { get; set; } = true;
    }

    public class CustomYearValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int year)
            {
                return year <= DateTime.Now.Year;
            }
            return false;
        }
    }
}
