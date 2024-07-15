using System.ComponentModel.DataAnnotations;

namespace SmartHouse.Models
{
    public class Medicao
    {
        [Required]
        public int MedicaoId { get; set; }
      
        [Required]
        public int Porta { get; set; }

        [Required]
        public int Movimento { get; set; }

        [Required]
        public System.DateTime DataMedicao { get; set; }


    }
}
