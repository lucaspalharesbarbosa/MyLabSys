using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("ResultadosExamesOrdensServicos")]
    public class ResultadoExameOrdemServico {
        public int Id { get; set; }

        public int IdExameOrdemServico { get; set; }
        public ExameOrdemServico ExameOrdemServico { get; set; }

        [MaxLength]
        public string DescricaoResultado { get; set; }
    }
}