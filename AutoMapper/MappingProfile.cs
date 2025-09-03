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
            CreateMap<Beacon, BeaconDTO>();
            CreateMap<Cliente, ClienteDTO>();
            CreateMap<Localizacao, LocalizacaoDTO>();
            CreateMap<ModeloMoto, ModeloMotoDTO>();
            CreateMap<Moto, MotoDTO>();
            CreateMap<Movimentacao, MovimentacaoDTO>();
            CreateMap<TipoMovimentacao, TipoMovimentacaoDTO>();
            CreateMap<Usuario, UsuarioDTO>();

            // DTOs de resposta adicionais (estão em DTOs/Common)
            CreateMap<Patio, PatiosDTO>();
            CreateMap<ModeloBeacon, ModeloBeaconDTO>();
            CreateMap<RegistroBateria, RegistroBateriaDTO>();
            CreateMap<TipoUsuario, TipoUsuarioDTO>();

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
        }
    }

    // Helper para updates parciais: NÃO sobrescreve destino com null
    public static class MappingExtensions
    {
        public static IMappingExpression<TSrc, TDest> IgnoreNulls<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> map)
        {
            map.ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
            return map;
        }
    }
}
