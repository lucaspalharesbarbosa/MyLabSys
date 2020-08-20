using MyLabSys.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("Pacientes")]
    public class Paciente {
        public Paciente() {
            OrdensServicos = new HashSet<OrdemServico>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Nome { get; set; }


        public DateTime DataNascimento { get; set; }

        public SexoPaciente Sexo { get; set; }

        [StringLength(255)]
        public string Endereco { get; set; }

        public IEnumerable<OrdemServico> OrdensServicos { get; set; }
    }
}