using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("Exames")]
    public class Exame {
        public int Id { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Preco { get; set; }
    }
}