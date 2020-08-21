using System;
using System.ComponentModel.DataAnnotations;

namespace MyLabSys.ViewModels {
    public class ItemOrdemServicoGridModel {
        public int Id { get; set; }

        [Display(Name = "Protocolo")]
        public string CodigoProtocolo { get; set; }

        [Display(Name = "Código do pedido")]
        public string CodigoPedidoMedico { get; set; }

        [Display(Name = "Emissão")]
        public DateTime DataEmissao { get; set; }
        public string DataEmissaoFormatada => DataEmissao.ToShortDateString();

        [Display(Name = "Previsao de entrega")]
        public DateTime DataPrevisaoEntrega { get; set; }
        public string DataPrevisaoEntregaFormatada => DataPrevisaoEntrega.ToShortDateString();

        [Display(Name = "Convênio")]
        public string NomeConvenio { get; set; }

        [Display(Name = "Paciente")]
        public string NomePaciente { get; set; }
    }
}