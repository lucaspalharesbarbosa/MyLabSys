using MyLabSys.Models;
using MyLabSys.ViewModels.Dtos;

namespace MyLabSys.Services.Interfaces {
    public interface IOrdemServicoService {
        bool Incluir(OrdemServicoDto ordemServicoDto);
        OrdemServico[] ObterOrdensServicos(string codigoProtocolo);
    }
}