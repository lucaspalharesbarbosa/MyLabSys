using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("Convenios")]
    public class Convenio {
        public Convenio() {
            Pacientes = new HashSet<Paciente>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PercentualDesconto { get; set; }

        public ICollection<Paciente> Pacientes { get; set; }
    }
}