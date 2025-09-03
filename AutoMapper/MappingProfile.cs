using AutoMapper;
using API_.Net.Models;
using API_.Net.DTOs;               // BeaconDTO, ClienteDTO, ...
using API_.Net.DTOs.Requests;      // Create*/Update* DTOs

namespace API_.Net.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -------- Entity -> DTO (saída) --------
            CreateMap<Beacon, BeaconDto>();
            CreateMap<Cliente, ClienteDto>();
            CreateMap<Localizacao, LocalizacaoDto>();
            CreateMap<ModeloMoto, ModeloMotoDto>();
            CreateMap<Moto, MotoDto>();
            CreateMap<Movimentacao, MovimentacaoDto>();
            CreateMap<TipoMovimentacao, TipoMovimentacaoDto>();
            CreateMap<Usuario, UsuarioDto>();

            // comuns/adicionais
            CreateMap<Patio, PatioDTO>();
            CreateMap<ModeloBeacon, ModeloBeaconDto>();
            CreateMap<RegistroBateria, RegistroBateriaDto>();
            CreateMap<TipoUsuario, TipoUsuarioDto>();
            CreateMap<Departamento, DepartamentoDto>();
            CreateMap<Filial, FilialDTO>();
            CreateMap<Funcionario, FuncionarioDto>();

            // -------- Create DTO -> Entity (entrada) --------
            CreateMap<CreateBeaconDTO, Beacon>();
            CreateMap<CreateClienteDTO, Cliente>();
            CreateMap<CreateLocalizacaoDTO, Localizacao>();
            CreateMap<CreateModeloBeaconDTO, ModeloBeacon>();
            CreateMap<CreateModeloMotoDTO, ModeloMoto>();
            CreateMap<CreateMotoDTO, Moto>();
            CreateMap<CreateMovimentacaoDTO, Movimentacao>();
            CreateMap<CreatePatioDTO, Patio>();
            CreateMap<CreateRegistroBateriaDTO, RegistroBateria>();
            CreateMap<CreateTipoMovimentacaoDTO, TipoMovimentacao>();
            CreateMap<CreateTipoUsuarioDTO, TipoUsuario>();
            CreateMap<CreateUsuarioDTO, Usuario>();
            CreateMap<CreateDepartamentoDTO, Departamento>();
            CreateMap<CreateFilialDTO, Filial>();
            CreateMap<CreateFuncionarioDTO, Funcionario>();

            // -------- Update DTO -> Entity (ignora nulls) --------
            CreateMap<UpdateBeaconDTO, Beacon>().IgnoreNulls();
            CreateMap<UpdateClienteDTO, Cliente>().IgnoreNulls();
            CreateMap<UpdateLocalizacaoDTO, Localizacao>().IgnoreNulls();
            CreateMap<UpdateModeloBeaconDTO, ModeloBeacon>().IgnoreNulls();
            CreateMap<UpdateModeloMotoDTO, ModeloMoto>().IgnoreNulls();
            CreateMap<UpdateMotoDTO, Moto>().IgnoreNulls();
            CreateMap<UpdateMovimentacaoDTO, Movimentacao>().IgnoreNulls();
            CreateMap<UpdatePatioDTO, Patio>().IgnoreNulls();
            CreateMap<UpdateRegistroBateriaDTO, RegistroBateria>().IgnoreNulls();
            CreateMap<UpdateTipoMovimentacaoDTO, TipoMovimentacao>().IgnoreNulls();
            CreateMap<UpdateTipoUsuarioDTO, TipoUsuario>().IgnoreNulls();
            CreateMap<UpdateUsuarioDTO, Usuario>().IgnoreNulls();
            CreateMap<UpdateDepartamentoDTO, Departamento>().IgnoreNulls();
            CreateMap<UpdateFilialDTO, Filial>().IgnoreNulls();
            CreateMap<UpdateFuncionarioDTO, Funcionario>().IgnoreNulls();
        }
    }

    // Helper para updates parciais: não sobrescreve destino com null
    public static class MappingExtensions
    {
        public static IMappingExpression<TSrc, TDest> IgnoreNulls<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> map)
        {
            // ForAllMembers retorna void na sua versão — chame e depois retorne o map
            map.ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
            return map;
        }
    }
}
