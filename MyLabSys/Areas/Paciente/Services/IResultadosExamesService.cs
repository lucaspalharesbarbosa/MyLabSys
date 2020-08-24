using MyLabSys.Areas.Paciente.ViewModels;

namespace MyLabSys.Areas.Paciente.Services.Interfaces {
    public interface IResultadosExamesService {
        bool ValidarUsuarioESenhaSaoValidos(string usuario, string senha);
        ReportResultadosExamesViewModel ObterResultadosExames(string usuario, string senha);
    }
}