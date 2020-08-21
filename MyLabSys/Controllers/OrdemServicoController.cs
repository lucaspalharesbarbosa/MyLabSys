using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLabSys.Models;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels;
using MyLabSys.ViewModels.Dtos;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        public IActionResult Create() {
            ViewData["PacientesSelectList"] = new SelectList(_db.Pacientes, "Id", "Nome");
            ViewData["MedicosSelectList"] = new SelectList(_db.Medicos, "Id", "Nome");
            ViewData["PostosColetasSelectList"] = new SelectList(_db.PostosColetas, "Id", "Descricao");
            ViewData["ExamesSelectList"] = ObterExamesSelectList();

            return View(new OrdemServicoViewModel {
                DataEmissao = DateTime.Today,
                DataPrevisaoEntrega = null
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdemServicoViewModel viewModel) {
            if (!ModelState.IsValid) {
                ViewData["PacientesSelectList"] = new SelectList(_db.Pacientes, "Id", "Nome", viewModel.IdPaciente);
                ViewData["MedicosSelectList"] = new SelectList(_db.Medicos, "Id", "Nome", viewModel.IdMedico);
                ViewData["PostosColetasSelectList"] = new SelectList(_db.PostosColetas, "Id", "Descricao", viewModel.IdPostoColeta);
                ViewData["ExamesSelectList"] = ObterExamesSelectList();

                return View(viewModel);
            }

            var ordemServicoDto = new OrdemServicoDto {
                IdPaciente = viewModel.IdPaciente.Value,
                IdMedico = viewModel.IdMedico.Value,
                IdPostoColeta = viewModel.IdPostoColeta.Value,
                CodigoProtocolo = viewModel.CodigoProtocolo,
                CodigoPedidoMedico = viewModel.CodigoPedidoMedico,
                DataEmissao = viewModel.DataEmissao,
                DataPrevisaoEntrega = viewModel.DataPrevisaoEntrega,
                NomeConvenio = viewModel.NomeConvenio,
                IdsExames = viewModel.IdsExames
            };

            _service.Incluir(ordemServicoDto);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private SelectListItem[] ObterExamesSelectList() {
            return _db.Exames
                .Select(exame => new SelectListItem {
                    Value = exame.Id.ToString(),
                    Text = exame.Descricao
                }).ToArray();
        }
    }
}