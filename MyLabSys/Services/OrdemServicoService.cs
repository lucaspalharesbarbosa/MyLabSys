using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLabSys.Models;
using MyLabSys.Models.Enums;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels.Dtos;
using System;
using System.Linq;

namespace MyLabSys.Services {
    public class OrdemServicoService : IOrdemServicoService {
        private readonly MyLabSysContext _db;

        public OrdemServicoService(MyLabSysContext db) {
            _db = db;
        }

        public void Salvar(OrdemServicoDto ordemServicoDto) {
            var ordemServicoNaoCadastrada = ordemServicoDto.Id == 0;

            if (ordemServicoNaoCadastrada) {
                Incluir(ordemServicoDto);
            } else {
                Editar(ordemServicoDto);
            }
        }

        private void Incluir(OrdemServicoDto ordemServicoDto) {
            var ordemServico = new OrdemServico {
                IdPaciente = ordemServicoDto.IdPaciente,
                IdMedico = ordemServicoDto.IdMedico,
                IdPostoColeta = ordemServicoDto.IdPostoColeta,
                CodigoProtocolo = GerarCodigoProtocolo(ordemServicoDto.CodigoProtocolo, ordemServicoDto.IdPaciente),
                CodigoPedidoMedico = ordemServicoDto.CodigoPedidoMedico,
                DataEmissao = ordemServicoDto.DataEmissao,
                DataPrevisaoEntrega = ordemServicoDto.DataPrevisaoEntrega.Value,
                Status = StatusOrdemServico.Aberta
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

        private string GerarCodigoProtocolo(string codigoProtocolo, int idPaciente) {
            var informouCodigoProtocolo = !string.IsNullOrEmpty(codigoProtocolo);

            if (informouCodigoProtocolo) {
                return codigoProtocolo;
            }

            var nomePaciente = _db.Pacientes
                .Where(p => p.Id == idPaciente)
                .Select(p => p.Nome)
                .First();
            var nomesSeparados = nomePaciente.Split(" ");
            var iniciaisNomePaciente = string.Empty;

            foreach (var nome in nomesSeparados) {
                iniciaisNomePaciente += nome.Substring(0, 1);
            }

            var random = new Random();
            var codigoProtocoloGerado = iniciaisNomePaciente + random.Next(9999);

            return codigoProtocoloGerado;
        }

        private void Editar(OrdemServicoDto ordemServicoDto) {
            var ordemServico = _db.OrdensServicos
                .Where(o => o.Id == ordemServicoDto.Id)
                .Include(o => o.Exames)
                .First();

            ordemServico.IdMedico = ordemServicoDto.IdMedico;
            ordemServico.IdPostoColeta = ordemServicoDto.IdPostoColeta;
            ordemServico.IdPaciente = ordemServicoDto.IdPaciente;
            ordemServico.CodigoProtocolo = GerarCodigoProtocolo(ordemServicoDto.CodigoProtocolo, ordemServicoDto.IdPaciente);
            ordemServico.CodigoPedidoMedico = ordemServicoDto.CodigoPedidoMedico;
            ordemServico.DataEmissao = ordemServicoDto.DataEmissao;
            ordemServico.DataPrevisaoEntrega = ordemServicoDto.DataPrevisaoEntrega.Value;

            atualizarExames();

            void atualizarExames() {
                var idsExamesExistentes = ordemServico.Exames
                    .Select(exame => exame.Id)
                    .ToArray();
                var idsExamesIncluir = ordemServicoDto.IdsExames
                    .Where(idExame => !idsExamesExistentes.Contains(idExame))
                    .ToArray();
                var examesIncluir = _db.Exames
                    .Where(exame => idsExamesIncluir.Contains(exame.Id))
                    .ToArray();

                foreach (var exameIncluir in examesIncluir) {
                    _db.ExamesOrdensServicos.Add(new ExameOrdemServico {
                        IdOrdemServico = ordemServico.Id,
                        IdExame = exameIncluir.Id,
                        Preco = exameIncluir.Preco
                    });
                }
                var idsTodosExames = _db.Exames.Select(e => e.Id).ToArray();

                var examesExcluir = _db.ExamesOrdensServicos
                    .Where(exameOrdem => exameOrdem.IdOrdemServico == ordemServicoDto.Id
                        && !ordemServicoDto.IdsExames.Contains(exameOrdem.IdExame)
                        && !idsExamesIncluir.Contains(exameOrdem.IdExame))
                    .ToArray();

                foreach (var exameExcluir in examesExcluir) {
                    _db.Remove(exameExcluir);
                }
            }
        }

        public void Excluir(int id) {
            var ordemServico = _db.OrdensServicos
                .Where(o => o.Id == id)
                .Include(o => o.Exames)
                .FirstOrDefault();
            var ordemServicoExiste = ordemServico != null;

            if (ordemServicoExiste) {
                _db.Remove(ordemServico);
            }
        }

        public void Fechar(int id) {
            var ordemServico = _db.OrdensServicos.Find(id);

            ordemServico.Status = StatusOrdemServico.Fechada;

            _db.Update(ordemServico);
        }

        public void Reabrir(int id) {
            var ordemServico = _db.OrdensServicos.Find(id);

            ordemServico.Status = StatusOrdemServico.Aberta;

            _db.Update(ordemServico);
        }

        public OrdemServicoDto[] ObterDadosOrdensServicos(int? id = null) {
            var queryOrdensServicos = _db.OrdensServicos.AsQueryable();

            var temId = id != null && id > 0;
            if (temId) {
                queryOrdensServicos = queryOrdensServicos.Where(o => o.Id == id.Value).AsQueryable();
            }

            return queryOrdensServicos
                .Select(ordemServico => new OrdemServicoDto {
                    Id = ordemServico.Id,
                    IdPaciente = ordemServico.IdPaciente,
                    IdMedico = ordemServico.IdMedico,
                    IdPostoColeta = ordemServico.IdPostoColeta,
                    CodigoProtocolo = ordemServico.CodigoProtocolo,
                    CodigoPedidoMedico = ordemServico.CodigoPedidoMedico,
                    NomeConvenio = ordemServico.Paciente.Convenio.Nome,
                    DataEmissao = ordemServico.DataEmissao,
                    DataPrevisaoEntrega = ordemServico.DataPrevisaoEntrega,
                    NomePaciente = ordemServico.Paciente.Nome,
                    NomeMedico = ordemServico.Medico.Nome,
                    IdsExames = ordemServico.Exames.Select(e => e.IdExame),
                    Status = ordemServico.Status,
                    EstaAberta = ordemServico.Status == StatusOrdemServico.Aberta
                }).ToArray();
        }

        public OrdemServicoDto[] ObterDadosOrdensServicosPorProtocolo(string codigoProtocolo) {
            var ordensServicos = ObterDadosOrdensServicos();
            var temCodigoProtocolo = !string.IsNullOrEmpty(codigoProtocolo);

            if (temCodigoProtocolo) {
                ordensServicos = ordensServicos
                    .Where(ordem => ordem.CodigoProtocolo.Contains(codigoProtocolo))
                    .ToArray();
            }

            return ordensServicos;
        }
    }
}