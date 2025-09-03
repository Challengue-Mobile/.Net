using AutoMapper;
using API_.Net.Models;
using API_.Net.DTOs;

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

            // DTOs de resposta adicionais (arquivos que você já tem em DTOs/Common)
            CreateMap<Patio, PatioDTO>();
            CreateMap<ModeloBeacon, ModeloBeaconDTO>();
            CreateMap<RegistroBateria, RegistroBateriaDTO>();
            CreateMap<TipoUsuario, TipoUsuarioDTO>();

            // -------- Create DTO -> Entity (entrada) --------
            CreateMap<CreateBeaconDto, Beacon>();
            CreateMap<CreateClienteDto, Cliente>();
            CreateMap<CreateLocalizacaoDto, Localizacao>();
            CreateMap<CreateModeloMotoDto, ModeloMoto>();
            CreateMap<CreateMotoDto, Moto>();
            CreateMap<CreateMovimentacaoDto, Movimentacao>();
            CreateMap<CreateTipoMovimentacaoDto, TipoMovimentacao>();
            CreateMap<CreateUsuarioDto, Usuario>();

            // -------- Update DTO -> Entity (ignora nulls) --------
            CreateMap<UpdateBeaconDto, Beacon>().IgnoreNulls();
            CreateMap<UpdateClienteDto, Cliente>().IgnoreNulls();
            CreateMap<UpdateModeloMotoDto, ModeloMoto>().IgnoreNulls();
            CreateMap<UpdateMotoDto, Moto>().IgnoreNulls();
            CreateMap<UpdateMovimentacaoDto, Movimentacao>().IgnoreNulls();
            CreateMap<UpdateTipoMovimentacaoDto, TipoMovimentacao>().IgnoreNulls();
            CreateMap<UpdateUsuarioDto, Usuario>().IgnoreNulls();
        }
    }

    // Helper para updates parciais: não sobrescreve com null
    public static class MappingExtensions
    {
        public static IMappingExpression<TSrc, TDest> IgnoreNulls<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> map)
        {
            // Usa overload amplo (src, dest, member, ctx) para compatibilidade com versões do AutoMapper
            map.ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember, ctx) => srcMember != null));

            return map;
        }
    }
}
