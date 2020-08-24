using System.Collections.Generic;

namespace MyLabSys.Areas.Paciente.ViewModels {
    public class ReportResultadosExamesViewModel {
        public string NomePaciente { get; set; }
        public string NomeMedico { get; set; }
        public string DescricaoPostoColeta { get; set; }
        public string DataPrevisaoEntrega { get; set; }
        public bool ResultadoEstaDisponivel { get; set; }
        public IEnumerable<ExameReportResultadosExamesViewModel> ResultadosExames { get; set; }
    }
}