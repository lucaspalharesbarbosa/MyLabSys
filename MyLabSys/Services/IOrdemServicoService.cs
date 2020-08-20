using MyLabSys.Models;

namespace MyLabSys.Services.Interfaces {
    public interface IOrdemServicoService {
        OrdemServico[] ObterOrdensServicos(string codigoProtocolo);
    }
}