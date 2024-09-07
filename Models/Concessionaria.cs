using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConcessionariaMVC.Models
{
    public class Concessionaria
    {
        public int ConcessionariaID { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo Nome deve ter no máximo 100 caracteres.")]
        [Index(IsUnique = true)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Endereço é obrigatório.")]
        [MaxLength(100, ErrorMessage = "Endereço deve ter no máximo 100 caracteres.")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo Cidade deve ter no máximo 50 caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo Estado é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo Estado deve ter no máximo 50 caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O campo CEP é obrigatório.")]
        [MaxLength(10, ErrorMessage = "O campo CEP deve ter no máximo 10 caracteres.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
        [MaxLength(15, ErrorMessage = "O campo Telefone deve ter no máximo 15 caracteres.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve conter um endereço de email válido.")]
        [MaxLength(100, ErrorMessage = "O campo Email deve ter no máximo 100 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Capacidade Máxima de Veículos é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo Capacidade Máxima de Veículos deve ser maior que 0.")]
        public int CapacidadeMaximaVeiculos { get; set; }

        public bool BitAtivo { get; set; } = true;

        // Adicionar o campo RowVersion para controle de concorrência otimista
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
