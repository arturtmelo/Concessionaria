using ConcessionariaMVC.Data;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcessionariaMVC.Models
{
    public class Venda
    {
        [Key]
        public int VendaID { get; set; }

        [Required]
        [ForeignKey("Veiculo")]
        public int VeiculoID { get; set; }
        public Veiculo Veiculo { get; set; }

        [Required]
        [ForeignKey("Concessionaria")]
        public int ConcessionariaID { get; set; }
        public Concessionaria Concessionaria { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }
        public Cliente Cliente { get; set; }

        // Campo que exibe o nome do fabricante associado ao veículo
        [NotMapped] // Não persistir no banco de dados
        public string NomeFabricante => Veiculo?.Fabricante?.Nome;

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data Venda")]
        [CustomValidation(typeof(Venda), "ValidateDataVenda")]
        public DateTime DataVenda { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da venda deve ser positivo.")]
        [Display(Name = "Valor Venda")]
        [CustomValidation(typeof(Venda), "ValidatePrecoVenda")]
        public decimal PrecoVenda { get; set; }

        [StringLength(20)]
        [Display(Name = "Protocolo Venda")]
        public string ProtocoloVenda { get; set; }

        public bool BitAtivo { get; set; } = true;

        // Validação para garantir que a data da venda não seja futura
        public static ValidationResult ValidateDataVenda(DateTime dataVenda, ValidationContext context)
        {
            if (dataVenda > DateTime.Now)
            {
                return new ValidationResult("Data da venda não pode ser futura.");
            }
            return ValidationResult.Success;
        }

        // Validação do preço da venda, comparando com o preço do veículo
        public static ValidationResult ValidatePrecoVenda(decimal precoVenda, ValidationContext context)
        {
            var instance = context.ObjectInstance as Venda;
            if (instance == null)
            {
                return new ValidationResult("Erro ao validar o preço da venda.");
            }

            using (var db = new ApplicationDbContext())
            {
                var veiculo = db.Veiculos.FirstOrDefault(v => v.VeiculoID == instance.VeiculoID);
                if (veiculo != null && precoVenda > veiculo.Preco)
                {
                    return new ValidationResult("O valor da venda não pode ser superior ao preço do veículo cadastrado.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
