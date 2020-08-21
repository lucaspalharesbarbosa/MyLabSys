using MyLabSys.Models;
using MyLabSys.ViewModels.Dtos;

namespace MyLabSys.Services.Interfaces {
    public interface IOrdemServicoService {
        bool Salvar(OrdemServicoDto ordemServicoDto);
        OrdemServico[] ObterOrdensServicos(string codigoProtocolo = "");
    }
}