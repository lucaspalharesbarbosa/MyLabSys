using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyLabSys.ViewModels {
    public class OrdemServicoViewModel {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Protocolo é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Protocolo")]
        public string CodigoProtocolo { get; set; }

        [Required(ErrorMessage = "O campo Código do pedido do médico é obrigatório")]
        [StringLength(100)]
        [Display(Name = "Código do pedido do médico")]
        public string CodigoPedidoMedico { get; set; }

        [Display(Name = "Emissão")]
        public DateTime DataEmissao { get; set; }

        [Required(ErrorMessage = "O campo Previsao de entrega é obrigatório")]
        [Display(Name = "Previsao de entrega")]
        public DateTime? DataPrevisaoEntrega { get; set; }

        [Required(ErrorMessage = "O campo Médico é obrigatório")]
        [Display(Name = "Médico")]
        public int? IdMedico { get; set; }

        [Required(ErrorMessage = "O campo Posto de coleta é obrigatório")]
        [Display(Name = "Posto de coleta")]
        public int? IdPostoColeta { get; set; }

        [Required(ErrorMessage = "O campo Paciente é obrigatório")]
        [Display(Name = "Paciente")]
        public int? IdPaciente { get; set; }

        [Required(ErrorMessage = "O campo Convênio é obrigatório")]
        [Display(Name = "Convênio")]
        public string NomeConvenio { get; set; }

        [Required(ErrorMessage = "O campo Exames é obrigatório")]
        [Display(Name = "Exames")]
        public int[] IdsExames { get; set; }
    }
}