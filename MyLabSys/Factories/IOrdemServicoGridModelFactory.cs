using MyLabSys.ViewModels;

namespace MyLabSys.Factories.Interfaces {
    public interface IOrdemServicoGridModelFactory {
        OrdemServicoGridModel Build();
        OrdemServicoGridModel Build(string codigoProtocoloFiltrar);
    }
}