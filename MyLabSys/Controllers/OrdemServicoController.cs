using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult Index() {
            return View(new OrdemServicoGridModel {
                OrdensServicos = _service.ObterOrdensServicos()
                .Select(ordem => new ItemOrdemServicoGridModel {
                    Id = ordem.Id,
                    CodigoProtocolo = ordem.CodigoProtocolo,
                    CodigoPedidoMedico = ordem.CodigoPedidoMedico,
                    DataEmissao = ordem.DataEmissao,
                    DataPrevisaoEntrega = ordem.DataPrevisaoEntrega,
                    NomeConvenio = ordem.NomeConvenio,
                    NomePaciente = ordem.Paciente.Nome
                }).ToArray()
            });
        }

        [HttpPost]
        public IActionResult Index(string codigoProtocoloFiltrar) {
            return View(new OrdemServicoGridModel {
                CodigoProtocoloFiltrar = codigoProtocoloFiltrar,
                OrdensServicos = _service.ObterOrdensServicos(codigoProtocoloFiltrar)
                .Select(ordem => new ItemOrdemServicoGridModel {
                    Id = ordem.Id,
                    CodigoProtocolo = ordem.CodigoProtocolo,
                    CodigoPedidoMedico = ordem.CodigoPedidoMedico,
                    DataEmissao = ordem.DataEmissao,
                    DataPrevisaoEntrega = ordem.DataPrevisaoEntrega,
                    NomeConvenio = ordem.NomeConvenio,
                    NomePaciente = ordem.Paciente.Nome
                }).ToArray()
            });
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
                ViewData["ExamesSelectList"] = ObterExamesSelectList(true);

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

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var ordemServico = _db.OrdensServicos
                .Include(o => o.Exames)
                .First();

            if (ordemServico == null) {
                return NotFound();
            }

            ViewData["PacientesSelectList"] = new SelectList(_db.Pacientes, "Id", "Nome", ordemServico.IdPaciente);
            ViewData["MedicosSelectList"] = new SelectList(_db.Medicos, "Id", "Nome", ordemServico.IdMedico);
            ViewData["PostosColetasSelectList"] = new SelectList(_db.PostosColetas, "Id", "Descricao", ordemServico.IdPostoColeta);
            ViewData["ExamesSelectList"] = ObterExamesSelectList(true);

            return View("Create", new OrdemServicoViewModel {
                Id = ordemServico.Id,
                IdPaciente = ordemServico.IdPaciente,
                IdMedico = ordemServico.IdMedico,
                IdPostoColeta = ordemServico.IdPostoColeta,
                CodigoProtocolo = ordemServico.CodigoProtocolo,
                CodigoPedidoMedico = ordemServico.CodigoPedidoMedico,
                DataEmissao = ordemServico.DataEmissao,
                DataPrevisaoEntrega = ordemServico.DataPrevisaoEntrega,
                NomeConvenio = ordemServico.CodigoPedidoMedico,
                IdsExames = ordemServico.Exames
                    .Select(e => e.Id)
                    .ToArray()
            });
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