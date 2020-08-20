using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("Medicos")]
    public class Medico {
        public Medico() {
            OrdensServicos = new HashSet<OrdemServico>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        [StringLength(150)]
        public string Especialidade { get; set; }

        public IEnumerable<OrdemServico> OrdensServicos { get; set; }
    }
}