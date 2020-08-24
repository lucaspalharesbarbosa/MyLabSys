using System.ComponentModel.DataAnnotations;

namespace MyLabSys.Areas.Paciente.ViewModels {
    public class ResultadosExamesViewModel {
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Senha")]
        public string Senha { get; set; }

        public bool UsuarioESenhaInvalidos { get; set; }
    }
}