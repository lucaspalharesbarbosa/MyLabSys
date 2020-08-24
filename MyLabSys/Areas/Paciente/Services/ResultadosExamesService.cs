using MyLabSys.Areas.Paciente.Services.Interfaces;
using MyLabSys.Areas.Paciente.ViewModels;
using MyLabSys.Models;
using System;
using System.Linq;

namespace MyLabSys.Areas.Paciente.Services {
    public class ResultadosExamesService : IResultadosExamesService {
        private readonly MyLabSysContext _db;

        public ResultadosExamesService(MyLabSysContext db) {
            _db = db;
        }

        public ReportResultadosExamesViewModel ObterResultadosExames(string usuario, string senha) {
            var dadosOrdemServico = (
                from ordemServico in _db.OrdensServicos
                    .Where(ordem => ordem.CodigoProtocolo == usuario && ordem.SenhaPaciente == senha)
                select new {
                    NomePaciente = ordemServico.Paciente.Nome,
                    NomeMedico = ordemServico.Medico.Nome,
                    DescricaoPostoColeta = ordemServico.PostoColeta.Descricao,
                    ordemServico.DataPrevisaoEntrega,
                    ResultadoEstaDisponivel = DateTime.Today >= ordemServico.DataPrevisaoEntrega,
                    ResultadosExames = ordemServico.Exames.Select(exameOrdem => new {
                        NomeExame = exameOrdem.Exame.Descricao,
                        exameOrdem.ResultadoExame.DescricaoResultado
                    })
                }
            )
            .First();

            return new ReportResultadosExamesViewModel {
                NomePaciente = dadosOrdemServico.NomePaciente,
                NomeMedico = dadosOrdemServico.NomeMedico,
                DescricaoPostoColeta = dadosOrdemServico.DescricaoPostoColeta,
                DataPrevisaoEntrega = dadosOrdemServico.DataPrevisaoEntrega.ToShortDateString(),
                ResultadoEstaDisponivel = dadosOrdemServico.ResultadoEstaDisponivel,
                ResultadosExames = dadosOrdemServico.ResultadosExames
                    .Select(resultado => new ExameReportResultadosExamesViewModel {
                        NomeExame = resultado.NomeExame,
                        Descricao = dadosOrdemServico.ResultadoEstaDisponivel
                            ? resultado.DescricaoResultado
                            : $"O resultado do exame {resultado.NomeExame} ainda não está disponível. A previsão de entrega é {dadosOrdemServico.DataPrevisaoEntrega.ToShortDateString()}"
                    })
            };
        }

        public bool ValidarUsuarioESenhaSaoValidos(string usuario, string senha) {
            var usuarioESenhaSaoValidos = _db.OrdensServicos.Any(ordem => ordem.CodigoProtocolo == usuario && ordem.SenhaPaciente == senha);

            return usuarioESenhaSaoValidos;
        }
    }
}
