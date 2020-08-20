using Microsoft.EntityFrameworkCore;
using MyLabSys.Models;
using MyLabSys.Services.Interfaces;
using System.Linq;

namespace MyLabSys.Services {
    public class OrdemServicoService : IOrdemServicoService {
        private readonly MyLabSysContext _db;

        public OrdemServicoService(MyLabSysContext db) {
            _db = db;
        }

        public OrdemServico[] ObterOrdensServicos(string codigoProtocolo) {
            var ordensServicos = _db.OrdensServicos
                .Include(ordem => ordem.Paciente)
                .ToArray();

            var filtrarPorCodigoProtocolo = !string.IsNullOrEmpty(codigoProtocolo);
            if (filtrarPorCodigoProtocolo) {
                ordensServicos = ordensServicos
                    .Where(ordem => ordem.CodigoProtocolo.Contains(codigoProtocolo))
                    .ToArray();
            }

            return ordensServicos;
        }
    }
}