using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               // LocalizacaoDTO
using API_.Net.DTOs.Requests;      // CreateLocalizacaoDTO / UpdateLocalizacaoDTO
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples;

namespace API.Net.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui localizações de motos")]
    public class LocalizacoesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LocalizacoesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as localizações", Description = "Obtém todas as localizações registradas")]
        public async Task<ActionResult<IEnumerable<LocalizacaoDto>>> GetLocalizacoes()
        {
            var entities = await _context.Localizacoes
                .AsNoTracking()
                .Include(l => l.Moto)
                .Include(l => l.Patio)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<LocalizacaoDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma localização pelo ID", Description = "Retorna uma localização específica")]
        public async Task<ActionResult<LocalizacaoDto>> GetLocalizacao(int id)
        {
            var entity = await _context.Localizacoes
                .AsNoTracking()
                .Include(l => l.Moto)
                .Include(l => l.Patio)
                .FirstOrDefaultAsync(l => l.ID_LOCALIZACAO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<LocalizacaoDto>(entity));
        }

        
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca localizações por moto", Description = "Lista histórico de localizações de uma moto, mais recentes primeiro")]
        public async Task<ActionResult<IEnumerable<LocalizacaoDto>>> GetLocalizacoesByMoto(int motoId)
        {
            var entities = await _context.Localizacoes
                .AsNoTracking()
                .Where(l => l.ID_MOTO == motoId)
                .Include(l => l.Moto)
                .Include(l => l.Patio)
                .OrderByDescending(l => l.DATA_HORA)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<LocalizacaoDto>>(entities);
            return Ok(dtos);
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Registra uma nova localização", Description = "Cria um registro de localização de moto")]
        public async Task<ActionResult<LocalizacaoDto>> PostLocalizacao([FromBody] CreateLocalizacaoDto dto)
        {
            var entity = _mapper.Map<Localizacao>(dto);
            entity.DATA_HORA = DateTime.Now;

            _context.Localizacoes.Add(entity);
            await _context.SaveChangesAsync();

            // recarrega com includes para preencher PlacaMoto/NomePatio no retorno
            await _context.Entry(entity).Reference(e => e.Moto).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Patio).LoadAsync();

            var result = _mapper.Map<LocalizacaoDto>(entity);
            return CreatedAtAction(nameof(GetLocalizacao), new { id = entity.ID_LOCALIZACAO }, result);
        }

       
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma localização", Description = "Atualiza informações de uma localização existente")]
        public async Task<ActionResult<LocalizacaoDto>> PutLocalizacao(int id, [FromBody] UpdateLocalizacaoDto dto)
        {
            var entity = await _context.Localizacoes.FirstOrDefaultAsync(l => l.ID_LOCALIZACAO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            // recarrega relacionamentos para o retorno mapeado
            await _context.Entry(entity).Reference(e => e.Moto).LoadAsync();
            await _context.Entry(entity).Reference(e => e.Patio).LoadAsync();

            return Ok(_mapper.Map<LocalizacaoDto>(entity));
        }

       
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma localização", Description = "Remove permanentemente uma localização do sistema")]
        public async Task<IActionResult> DeleteLocalizacao(int id)
        {
            var entity = await _context.Localizacoes.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Localizacoes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
