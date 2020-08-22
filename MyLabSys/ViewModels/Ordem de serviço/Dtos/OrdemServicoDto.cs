using System;
using System.Collections.Generic;

namespace MyLabSys.ViewModels.Dtos {
    public class OrdemServicoDto {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdPostoColeta { get; set; }
        public string CodigoProtocolo { get; set; }
        public string CodigoPedidoMedico { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime? DataPrevisaoEntrega { get; set; }
        public IEnumerable<int> IdsExames { get; set; }
        public string NomePaciente { get; set; }
        public string NomeMedico { get; set; }
        public string NomeConvenio { get; set; }
    }
}