using Microsoft.AspNetCore.Mvc;

namespace MyLabSys.Areas.Paciente.Controllers {
    [Area("Paciente")]
    public class PacienteController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}