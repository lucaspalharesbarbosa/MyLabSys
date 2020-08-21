using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public bool Salvar(OrdemServicoDto ordemServicoDto) {
            var ordemServicoJaCadastrada = ordemServicoDto.Id > 0;

            if (ordemServicoJaCadastrada) {
                Incluir(ordemServicoDto);
            } else {
                Editar(ordemServicoDto);
            }

            return true;
        }

        private void Incluir(OrdemServicoDto ordemServicoDto) {
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
        }

        private void Editar(OrdemServicoDto ordemServicoDto) {
            var ordemServico = _db.OrdensServicos
                .Where(o => o.Id == ordemServicoDto.Id)
                .Include(o => o.Exames)
                .First();

            ordemServico.DataEmissao = ordemServicoDto.DataEmissao;
            ordemServico.DataPrevisaoEntrega = ordemServicoDto.DataPrevisaoEntrega.Value;
            ordemServico.IdMedico = ordemServicoDto.IdMedico;
            ordemServico.IdPostoColeta = ordemServicoDto.IdPostoColeta;
            ordemServico.IdPaciente = ordemServicoDto.IdPaciente;
            ordemServico.NomeConvenio = ordemServicoDto.NomeConvenio;

            atualizarExames();

            void atualizarExames() {
                var idsExamesExistentes = ordemServico.Exames
                    .Select(exame => exame.Id)
                    .ToArray();
                var idsExamesIncluir = ordemServicoDto.IdsExames
                    .Where(idExame => !idsExamesExistentes.Contains(idExame))
                    .ToArray();
                var exames = ordemServico.Exames
                    .Where(exame => ordemServicoDto.IdsExames.Contains(exame.Id))
                    .Select(exame => new ExameOrdemServico {
                        OrdemServico = ordemServico,
                        IdExame = exame.Id,
                        Preco = exame.Preco
                    }).ToArray();
                var examesIncluir = _db.Exames
                    .Where(exame => idsExamesIncluir.Contains(exame.Id))
                    .ToArray();

                foreach (var idExameIncluir in idsExamesIncluir) {
                    var exameIncluir = examesIncluir.First(e => e.Id == idExameIncluir);

                    var novoExame = new ExameOrdemServico {
                        OrdemServico = ordemServico,
                        Exame = exameIncluir,
                        Preco = exameIncluir.Preco
                    };
                }

                var idsExamesExcluir = idsExamesIncluir
                    .Where(idExame => !idsExamesExistentes.Contains(idExame))
                    .ToArray();
                var examesExcluir = _db.ExamesOrdensServicos
                    .Where(exame => exame.IdOrdemServico == ordemServicoDto.Id
                        && idsExamesExcluir.Contains(exame.Id))
                    .ToArray();

                foreach (var idExameExcluir in idsExamesExcluir) {
                    var exameExcluir = examesExcluir.First(e => e.Id == idExameExcluir);

                    _db.Remove(exameExcluir);
                }
            }
        }

        public OrdemServico[] ObterOrdensServicos(string codigoProtocolo = "") {
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