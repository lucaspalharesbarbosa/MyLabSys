using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLabSys.Factories.Interfaces;
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
        private readonly IOrdemServicoGridModelFactory _gridModelFactory;

        public OrdemServicoController(MyLabSysContext db, IOrdemServicoService service, IOrdemServicoGridModelFactory gridModelFactory) {
            _db = db;
            _service = service;
            _gridModelFactory = gridModelFactory;
        }

        public IActionResult Index() {
            return View(_gridModelFactory.Build());
        }

        [HttpPost]
        public IActionResult Index(string codigoProtocoloFiltrar) {
            return View(_gridModelFactory.Build(codigoProtocoloFiltrar));
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
                Id = viewModel.Id,
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

            _service.Salvar(ordemServicoDto);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var ordemServico = _db.OrdensServicos
                .Where(o => o.Id == id.Value)
                .Include(o => o.Exames)
                .First();

            if (ordemServico == null) {
                return NotFound();
            }

            ViewData["PacientesSelectList"] = new SelectList(_db.Pacientes, "Id", "Nome", ordemServico.IdPaciente);
            ViewData["MedicosSelectList"] = new SelectList(_db.Medicos, "Id", "Nome", ordemServico.IdMedico);
            ViewData["PostosColetasSelectList"] = new SelectList(_db.PostosColetas, "Id", "Descricao", ordemServico.IdPostoColeta);
            ViewData["ExamesSelectList"] = ObterExamesSelectList(true);

            return View(nameof(Create), new OrdemServicoViewModel {
                Id = ordemServico.Id,
                IdPaciente = ordemServico.IdPaciente,
                IdMedico = ordemServico.IdMedico,
                IdPostoColeta = ordemServico.IdPostoColeta,
                CodigoProtocolo = ordemServico.CodigoProtocolo,
                CodigoPedidoMedico = ordemServico.CodigoPedidoMedico,
                DataEmissao = ordemServico.DataEmissao,
                DataPrevisaoEntrega = ordemServico.DataPrevisaoEntrega,
                NomeConvenio = ordemServico.NomeConvenio,
                IdsExames = ordemServico.Exames
                    .Select(e => e.IdExame)
                    .ToArray()
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            _service.Excluir(id.Value);

            await _db.SaveChangesAsync();

            return View(nameof(Index), _gridModelFactory.Build());
        }

        private SelectListItem[] ObterExamesSelectList(bool inicializar = false) {
            return _db.Exames
                .Select(exame => new {
                    exame.Id,
                    exame.Descricao,
                    exame.Preco
                })
                .ToArray()
                .Select(exame => new SelectListItem {
                    Value = exame.Id.ToString(),
                    Text = exame.Descricao + $" (Preço: {exame.Preco})",
                    Selected = inicializar
                }).ToArray();
        }
    }
}