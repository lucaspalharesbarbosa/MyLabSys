﻿using MyLabSys.ViewModels.Dtos;

namespace MyLabSys.Services.Interfaces {
    public interface IOrdemServicoService {
        void Salvar(OrdemServicoDto ordemServicoDto);
        void Excluir(int id);
        OrdemServicoDto[] ObterDadosOrdensServicos(int? id = null);
        OrdemServicoDto[] ObterDadosOrdensServicosPorProtocolo(string codigoProtocolo);
    }
}