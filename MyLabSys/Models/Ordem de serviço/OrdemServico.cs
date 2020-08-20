using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLabSys.Models {
    [Table("OrdensServicos")]
    public class OrdemServico {
        public OrdemServico() {
            Exames = new HashSet<ExameOrdemServico>();
        }

        public int Id { get; set; }

        public int NumeroPedidoMedico { get; set; }

        public int NumeroProtocolo { get; set; }

        public DateTime DataEmissao { get; set; }

        public DateTime DataPrevisaoEntrega { get; set; }

        public int IdPaciente { get; set; }
        public Paciente Paciente { get; set; }

        [StringLength(100)]
        public string NomeConvenio { get; set; }

        public int IdPostoColeta { get; set; }
        public PostoColeta PostoColeta { get; set; }

        public int IdMedico { get; set; }
        public Medico Medico { get; set; }

        public IEnumerable<ExameOrdemServico> Exames { get; set; }
    }
}