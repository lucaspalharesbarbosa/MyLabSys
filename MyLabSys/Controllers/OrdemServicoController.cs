using Microsoft.AspNetCore.Mvc;
using MyLabSys.Models;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels;
using System.Data;
using System.Linq;

namespace MyLabSys.Controllers {
    public class OrdemServicoController : Controller {
        private readonly MyLabSysContext _db;
        private readonly IOrdemServicoService _service;

        public OrdemServicoController(MyLabSysContext db, IOrdemServicoService service) {
            _db = db;
            _service = service;
        }

        public IActionResult Index(string codigoProtocolo) {
            var ordensServicosGridModel = _service.ObterOrdensServicos(codigoProtocolo)
                .Select(ordem => new OrdemServicoGridModel {
                    Id = ordem.Id,
                    CodigoProtocolo = ordem.CodigoProtocolo,
                    CodigoPedidoMedico = ordem.CodigoPedidoMedico,
                    DataEmissao = ordem.DataEmissao,
                    DataPrevisaoEntrega = ordem.DataPrevisaoEntrega,
                    NomeConvenio = ordem.NomeConvenio,
                    NomePaciente = ordem.Paciente.Nome
                }).ToArray();

            return View(ordensServicosGridModel);
        }
    }
}