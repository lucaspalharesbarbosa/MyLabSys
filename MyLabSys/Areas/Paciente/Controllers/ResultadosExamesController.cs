using Microsoft.AspNetCore.Mvc;
using MyLabSys.Areas.Paciente.Services.Interfaces;
using MyLabSys.Areas.Paciente.ViewModels;

namespace MyLabSys.Areas.Paciente.Controllers {
    [Area("Paciente")]
    [Route("ResultadosExames")]
    public class ResultadosExamesController : Controller {
        private readonly IResultadosExamesService _service;

        public ResultadosExamesController(IResultadosExamesService service) {
            _service = service;
        }

        public IActionResult Index() {
            return View(new ResultadosExamesViewModel());
        }

        [HttpPost]
        public IActionResult ConsultarResultadosExames(ResultadosExamesViewModel viewModel) {
            var usuarioESenhaInvalidos = !_service.ValidarUsuarioESenhaSaoValidos(viewModel.Usuario, viewModel.Senha);

            if(usuarioESenhaInvalidos) {
                return View(nameof(Index), new ResultadosExamesViewModel {
                    UsuarioESenhaInvalidos = true,
                    Usuario = viewModel.Usuario,
                    Senha = viewModel.Senha
                });
            }

            var resultadosExamesViewModel = _service.ObterResultadosExames(viewModel.Usuario, viewModel.Senha);

            return View("ReportResultadosExames", resultadosExamesViewModel);;
        }
    }
}