using System.ComponentModel.DataAnnotations;

namespace ConcessionariaMVC.Models
{
    public enum EnumTipoVeiculo
    {
        [Display(Name = "Carro")]
        Carro,

        [Display(Name = "Moto")]
        Moto,

        [Display(Name = "Caminhão")]
        Caminhao,

        [Display(Name = "Barco")]
        Barco,

        [Display(Name = "Monomotor")]
        Monomotor
    }
}
