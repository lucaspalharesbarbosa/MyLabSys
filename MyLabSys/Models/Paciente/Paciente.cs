using MyLabSys.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("Pacientes")]
    public class Paciente {
        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }

        public SexoPaciente Sexo { get; set; }

        [StringLength(255)]
        public string Endereco { get; set; }
    }
}