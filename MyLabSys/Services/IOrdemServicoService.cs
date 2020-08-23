using MyLabSys.Models;
using MyLabSys.ViewModels.Dtos;

namespace MyLabSys.Services.Interfaces {
    public interface IOrdemServicoService {
        OrdemServico Salvar(OrdemServicoDto ordemServicoDto);
        void Excluir(int id);
        void Fechar(OrdemServicoDto ordemServicoDto);
        void Reabrir(int id);
        OrdemServicoDto[] ObterDadosOrdensServicos(int? id = null);
        OrdemServicoDto[] ObterDadosOrdensServicosPorProtocolo(string codigoProtocolo);
    }
}