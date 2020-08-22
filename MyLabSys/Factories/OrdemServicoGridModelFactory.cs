using MyLabSys.Factories.Interfaces;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels;
using System.Linq;

namespace MyLabSys.Factories {
    public class OrdemServicoGridModelFactory : IOrdemServicoGridModelFactory {
        private readonly IOrdemServicoService _service;

        public OrdemServicoGridModelFactory(IOrdemServicoService service) {
            _service = service;
        }

        public OrdemServicoGridModel Build() {
            return BuildImpl();
        }

        public OrdemServicoGridModel Build(string codigoProtocoloFiltrar) {
            return BuildImpl(codigoProtocoloFiltrar);
        }

        private OrdemServicoGridModel BuildImpl(string codigoProtocoloFiltrar = "") {
            return new OrdemServicoGridModel {
                CodigoProtocoloFiltrar = codigoProtocoloFiltrar,
                OrdensServicos = _service.ObterDadosOrdensServicosPorProtocolo(codigoProtocoloFiltrar)
                    .Select(ordem => new ItemOrdemServicoGridModel {
                        Id = ordem.Id,
                        CodigoProtocolo = ordem.CodigoProtocolo,
                        CodigoPedidoMedico = ordem.CodigoPedidoMedico,
                        DataEmissao = ordem.DataEmissao,
                        DataPrevisaoEntrega = ordem.DataPrevisaoEntrega.Value,
                        NomeConvenio = ordem.NomeConvenio,
                        NomePaciente = ordem.NomePaciente
                    }).ToArray()
            };
        }
    }
}