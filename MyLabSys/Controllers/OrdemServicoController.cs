using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyLabSys.Factories.Interfaces;
using MyLabSys.Models;
using MyLabSys.Models.Enums;
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
                ViewData["PacientesSelectList"] = ObterPacientesSelectList(viewModel.IdPaciente);
                ViewData["MedicosSelectList"] = ObterMedicosSelectList(viewModel.IdMedico);
                ViewData["PostosColetasSelectList"] = ObterPostosColetasSelectList(viewModel.IdPostoColeta);
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

            var dadosOrdemServicoDto = _service.ObterDadosOrdensServicos(id.Value).FirstOrDefault();

            if (dadosOrdemServicoDto == null) {
                return NotFound();
            }

            ViewData["PacientesSelectList"] = ObterPacientesSelectList(dadosOrdemServicoDto.IdPaciente);
            ViewData["MedicosSelectList"] = ObterMedicosSelectList(dadosOrdemServicoDto.IdMedico);
            ViewData["PostosColetasSelectList"] = ObterPostosColetasSelectList(dadosOrdemServicoDto.IdPostoColeta);
            ViewData["ExamesSelectList"] = ObterExamesSelectList(true);

            return View(nameof(Create), new OrdemServicoViewModel {
                Id = dadosOrdemServicoDto.Id,
                IdPaciente = dadosOrdemServicoDto.IdPaciente,
                NomeConvenioPaciente = $"{dadosOrdemServicoDto.NomeConvenio} (Desconto: {dadosOrdemServicoDto.PercentualDescontoConvenio:0.##}%)",
                IdMedico = dadosOrdemServicoDto.IdMedico,
                IdPostoColeta = dadosOrdemServicoDto.IdPostoColeta,
                CodigoProtocolo = dadosOrdemServicoDto.CodigoProtocolo,
                CodigoPedidoMedico = dadosOrdemServicoDto.CodigoPedidoMedico,
                DataEmissao = dadosOrdemServicoDto.DataEmissao,
                DataPrevisaoEntrega = dadosOrdemServicoDto.DataPrevisaoEntrega,
                IdsExames = dadosOrdemServicoDto.IdsExames.ToArray(),
                EstaAberta = dadosOrdemServicoDto.EstaAberta,
                PacienteTemConvenio = dadosOrdemServicoDto.TemConvenio,
                PercentualDescontoConvenio = dadosOrdemServicoDto.PercentualDescontoConvenio,
                ValorDescontoConvenio = dadosOrdemServicoDto.ValorDescontoConvenio,
                ValorTotal = dadosOrdemServicoDto.ValorTotal,
                UsuarioPaciente = dadosOrdemServicoDto.CodigoProtocolo,
                SenhaPaciente = dadosOrdemServicoDto.SenhaPaciente
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

        [HttpPost]
        public async Task<IActionResult> Fechar(OrdemServicoViewModel viewModel) {
            if (!ModelState.IsValid) {
                ViewData["PacientesSelectList"] = ObterPacientesSelectList(viewModel.IdPaciente);
                ViewData["MedicosSelectList"] = ObterMedicosSelectList(viewModel.IdMedico);
                ViewData["PostosColetasSelectList"] = ObterPostosColetasSelectList(viewModel.IdPostoColeta);
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
                IdsExames = viewModel.IdsExames
            };

            _service.Fechar(ordemServicoDto);

            await _db.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost]
        public async Task<IActionResult> Reabrir(int? id) {
            if (id == null) {
                return NotFound();
            }

            _service.Reabrir(id.Value);

            await _db.SaveChangesAsync();

            return Ok(id);
        }

        public JsonResult ObterNomeConvenioPaciente(int idPaciente) {
            var dadosPaciente = _db.Pacientes
                .Where(p => p.Id == idPaciente)
                .Select(p => new {
                    NomeConvenio= p.Convenio.Nome,
                    PercentualDescontoConvenio = p.Convenio.PercentualDesconto,
                    TemConvenio = p.Convenio != null
                }).First();
            var nomeConvenio = dadosPaciente.TemConvenio
                ? $"{dadosPaciente.NomeConvenio} (Desconto: {dadosPaciente.PercentualDescontoConvenio:0.##}%)"
                : "Sem convênio";

            return Json(nomeConvenio);
        }

        private SelectList ObterPacientesSelectList(int? idPacienteSelecionado) {
            return new SelectList(_db.Pacientes, "Id", "Nome", idPacienteSelecionado);
        }

        private SelectList ObterMedicosSelectList(int? idMedicoSelecionado) {
            return new SelectList(_db.Medicos, "Id", "Nome", idMedicoSelecionado);
        }

        private SelectList ObterPostosColetasSelectList(int? idPostoColetaSelecionado) {
            return new SelectList(_db.PostosColetas, "Id", "Descricao", idPostoColetaSelecionado);
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