using Microsoft.EntityFrameworkCore;
using MyLabSys.Models;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels.Dtos;
using System.Linq;

namespace MyLabSys.Services {
    public class OrdemServicoService : IOrdemServicoService {
        private readonly MyLabSysContext _db;

        public OrdemServicoService(MyLabSysContext db) {
            _db = db;
        }

        public bool Incluir(OrdemServicoDto ordemServicoDto) {
            var ordemServico = new OrdemServico {
                IdPaciente = ordemServicoDto.IdPaciente,
                IdMedico = ordemServicoDto.IdMedico,
                IdPostoColeta = ordemServicoDto.IdPostoColeta,
                CodigoProtocolo = ordemServicoDto.CodigoProtocolo,
                CodigoPedidoMedico = ordemServicoDto.CodigoPedidoMedico,
                DataEmissao = ordemServicoDto.DataEmissao,
                DataPrevisaoEntrega = ordemServicoDto.DataPrevisaoEntrega.Value,
                NomeConvenio = ordemServicoDto.NomeConvenio
            };
            var exames = _db.Exames
                .Where(exame => ordemServicoDto.IdsExames.Contains(exame.Id))
                .Select(exame => new ExameOrdemServico {
                    OrdemServico = ordemServico,
                    IdExame = exame.Id,
                    Preco = exame.Preco
                }).ToArray();
            ordemServico.Exames = exames;

            _db.OrdensServicos.Add(ordemServico);

            return true;
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