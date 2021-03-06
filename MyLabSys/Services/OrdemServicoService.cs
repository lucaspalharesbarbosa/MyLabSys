﻿using Microsoft.EntityFrameworkCore;
using MyLabSys.Models;
using MyLabSys.Models.Enums;
using MyLabSys.Services.Interfaces;
using MyLabSys.ViewModels.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLabSys.Services {
    public class OrdemServicoService : IOrdemServicoService {
        private readonly MyLabSysContext _db;
        private const string HEMOGRAMA = "Hemograma";
        private const string GLICEMIA = "Glicemia";
        private const string EXAME_URINA = "Exame de urina";
        private const string EXAME_FEZES = "Exame de fezes";
        private const string TGO_TGP = "TGO (AST) TGP (ALP)";
        private const string COVID19 = "Teste COVID-19 (RT-PCR)";
        private const string VLDR = "VLDR";
        private const string COLESTEROL = "Colesteral";

        public OrdemServicoService(MyLabSysContext db) {
            _db = db;
        }

        public OrdemServico Salvar(OrdemServicoDto ordemServicoDto) {
            var ordemServicoNaoCadastrada = ordemServicoDto.Id == 0;

            OrdemServico ordemServico;

            if (ordemServicoNaoCadastrada) {
                ordemServico = Incluir(ordemServicoDto);
            } else {
                ordemServico = Editar(ordemServicoDto);
            }

            return ordemServico;
        }

        private OrdemServico Incluir(OrdemServicoDto ordemServicoDto) {
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

            return ordemServico;
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

        private OrdemServico Editar(OrdemServicoDto ordemServicoDto) {
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

            return ordemServico;

            void atualizarExames() {
                var idsExamesExistentes = ordemServico.Exames
                    .Select(exameOrdem => exameOrdem.IdExame)
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

        public void Fechar(OrdemServicoDto ordemServicoDto) {
            var ordemServico = Salvar(ordemServicoDto);

            ordemServico.SenhaPaciente = GerarSenhaPaciente(ordemServicoDto.CodigoProtocolo);
            ordemServico.Status = StatusOrdemServico.Fechada;

            GerarResultadosExames(ordemServicoDto);

            _db.Update(ordemServico);
        }

        private void GerarResultadosExames(OrdemServicoDto ordemServicoDto) {
            var dadosExamesOrdemServico = _db.ExamesOrdensServicos
                .Where(exameOrdem => exameOrdem.IdOrdemServico == ordemServicoDto.Id)
                .Select(exameOrdem => new {
                    exameOrdem.Id,
                    exameOrdem.IdExame,
                    exameOrdem.Exame.Descricao,
                    exameOrdem.Exame.Preco
                }).ToArray();


            var stringBuilder = new StringBuilder();
            string descricaoResultadoExame;

            foreach (var dadosExame in dadosExamesOrdemServico) {
                switch (dadosExame.Descricao) {
                    case HEMOGRAMA: {
                        stringBuilder.Append("Homoglobina: 15,1 ---------- 13 a 16 g/dL ............");
                        stringBuilder.Append("Hematrocrito: 44,8 ---------- 38 a 50%");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case GLICEMIA: {
                        stringBuilder.Append("Amosta em Jejum: 78 mg/dL ............");
                        stringBuilder.Append("Amosta após 1h: 135 mg/dL ............");
                        stringBuilder.Append("Amosta apos 2h: 121 mg/dL");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case EXAME_URINA: {
                        stringBuilder.Append("Cor: Amarelado ............");
                        stringBuilder.Append("Densidade: 1,04 ............");
                        stringBuilder.Append("PH: Ácida - 6,5 ............");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case EXAME_FEZES: {
                        stringBuilder.Append("Ovos de Ascaris Lumbricoides: Não detectado ............");
                        stringBuilder.Append("Larvas de Strongyloides Tercoralis: Não detectado");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case TGO_TGP: {
                        stringBuilder.Append("Leucócitos: 7,8405 mm3");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case COVID19: {
                        stringBuilder.Append("SARS COV2 RNA: POSITVO");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case VLDR: {
                        stringBuilder.Append("VLDR (Sífilis): NEGATIVO");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    case COLESTEROL: {
                        stringBuilder.Append("LDH (Ruim): 200 mg/dL");
                        stringBuilder.Append("HD (Bom): 60 mg/dL");

                        descricaoResultadoExame = stringBuilder.ToString();
                        stringBuilder.Clear();
                        break;
                    }
                    default:
                        descricaoResultadoExame = "Resultado do exame não disponível. Tente novamente mais tarde.";
                        stringBuilder.Clear();
                        break;
                }

                _db.ResultadosExamesOrdensServicos.Add(new ResultadoExameOrdemServico {
                    IdExameOrdemServico = dadosExame.Id,
                    DescricaoResultado = descricaoResultadoExame
                });
            }
        }

        private string GerarSenhaPaciente(string codigoProtocolo) {
            var random = new Random();

            return codigoProtocolo + random.Next(1000, 9999);
        }

        public void Reabrir(int id) {
            ExcluirResultadosExames(id);

            var ordemServico = _db.OrdensServicos.Find(id);

            ordemServico.SenhaPaciente = null;
            ordemServico.Status = StatusOrdemServico.Aberta;

            _db.Update(ordemServico);
        }

        private void ExcluirResultadosExames(int idOrdemServico) {
            var resultadosExames = _db.ExamesOrdensServicos
                .Where(exameOrdem => exameOrdem.IdOrdemServico == idOrdemServico)
                .Select(exameOrdem => exameOrdem.ResultadoExame)
                .ToArray();

            foreach (var resultadoExame in resultadosExames) {
                var existeResultadoExame = resultadoExame != null;

                if (existeResultadoExame) {
                    _db.Remove(resultadoExame);

                }
            }
        }

        public OrdemServicoDto[] ObterDadosOrdensServicos(int? id = null) {
            var queryOrdensServicos = _db.OrdensServicos.AsQueryable();

            var temId = id != null && id > 0;
            if (temId) {
                queryOrdensServicos = queryOrdensServicos.Where(o => o.Id == id.Value).AsQueryable();
            }

            return queryOrdensServicos
                .Select(ordemServico => new {
                    ordemServico.Id,
                    ordemServico.IdPaciente,
                    ordemServico.IdMedico,
                    ordemServico.IdPostoColeta,
                    ordemServico.CodigoProtocolo,
                    ordemServico.CodigoPedidoMedico,
                    NomeConvenio = ordemServico.Paciente.Convenio.Nome,
                    ordemServico.DataEmissao,
                    ordemServico.DataPrevisaoEntrega,
                    NomePaciente = ordemServico.Paciente.Nome,
                    NomeMedico = ordemServico.Medico.Nome,
                    IdsExames = ordemServico.Exames.Select(e => e.IdExame),
                    ordemServico.Status,
                    EstaAberta = ordemServico.Status == StatusOrdemServico.Aberta,
                    TemConvenio = ordemServico.Paciente.IdConvenio.HasValue,
                    ValorTotal = ordemServico.Exames.Sum(exame => exame.Preco),
                    PercentualDescontoConvenio = ordemServico.Paciente.Convenio.PercentualDesconto,
                    ordemServico.SenhaPaciente
                })
                .ToArray()
                .Select(ordemServico => {
                    var temConvenio = ordemServico.TemConvenio;
                    var valorDescontoConvenio = temConvenio
                        ? (ordemServico.PercentualDescontoConvenio / 100) * ordemServico.ValorTotal
                        : decimal.Zero;
                    var valorTotal = ordemServico.ValorTotal - valorDescontoConvenio;

                    return new OrdemServicoDto {
                        Id = ordemServico.Id,
                        IdPaciente = ordemServico.IdPaciente,
                        IdMedico = ordemServico.IdMedico,
                        IdPostoColeta = ordemServico.IdPostoColeta,
                        CodigoProtocolo = ordemServico.CodigoProtocolo,
                        CodigoPedidoMedico = ordemServico.CodigoPedidoMedico,
                        NomeConvenio = ordemServico.NomeConvenio,
                        DataEmissao = ordemServico.DataEmissao,
                        DataPrevisaoEntrega = ordemServico.DataPrevisaoEntrega,
                        NomePaciente = ordemServico.NomePaciente,
                        NomeMedico = ordemServico.NomeMedico,
                        IdsExames = ordemServico.IdsExames.ToArray(),
                        Status = ordemServico.Status,
                        EstaAberta = ordemServico.EstaAberta,
                        TemConvenio = ordemServico.TemConvenio,
                        PercentualDescontoConvenio = ordemServico.PercentualDescontoConvenio,
                        ValorDescontoConvenio = valorDescontoConvenio,
                        ValorTotal = valorTotal,
                        SenhaPaciente = ordemServico.SenhaPaciente
                    };
                })
                .ToArray();
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