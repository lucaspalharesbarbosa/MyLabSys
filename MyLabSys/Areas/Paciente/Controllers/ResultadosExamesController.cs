using Microsoft.AspNetCore.Mvc;
using MyLabSys.Areas.Paciente.ViewModels;

namespace MyLabSys.Areas.Paciente.Controllers {
    [Area("Paciente")]
    [Route("ResultadosExames")]
    public class ResultadosExamesController : Controller {
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public IActionResult ConsultarResultadosExames(ResultadosExamesViewModel viewModel) {
            return Ok(true);
        }
    }
}