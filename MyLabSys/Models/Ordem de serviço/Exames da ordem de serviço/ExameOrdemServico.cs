using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("ExamesOrdensServicos")]
    public class ExameOrdemServico {
        public int Id { get; set; }

        public int IdOrdemServico { get; set; }
        public OrdemServico OrdemServico { get; set; }

        public int IdExame { get; set; }
        public Exame Exame { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Preco { get; set; }
    }
}