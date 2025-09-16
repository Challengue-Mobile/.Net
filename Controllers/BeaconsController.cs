using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using API_.Net.Examples;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System;

using API_.Net.DTOs;              
using API_.Net.DTOs.Requests;
using API_.Net.DTOs.Common;  // ← ADICIONAR ESTA LINHA

namespace API.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui beacons")]
    public class BeaconsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeaconsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BeaconDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Lista todos os beacons com paginação",
                          Description = "Obtém uma lista paginada de todos os beacons cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<PagedResult<BeaconDto>>> GetBeacons(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) 
                return BadRequest(new { message = "Página deve ser maior que 0", field = "page" });
            
            if (pageSize < 1 || pageSize > 100) 
                return BadRequest(new { message = "PageSize deve estar entre 1 e 100", field = "pageSize" });

            var totalCount = await _context.Beacons.CountAsync();
            
            var entities = await _context.Beacons
                .AsNoTracking()
                .OrderBy(b => b.ID_BEACON)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<BeaconDto>>(entities);
            
            var result = new PagedResult<BeaconDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            };
            
            // Adicionar links HATEOAS para navegação
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/beacons";
            
            result.Links.Add(new Link 
            { 
                Href = $"{baseUrl}?page={page}&pageSize={pageSize}", 
                Rel = "self", 
                Method = "GET" 
            });
            
            if (result.HasPreviousPage)
            {
                result.Links.Add(new Link 
                { 
                    Href = $"{baseUrl}?page={page - 1}&pageSize={pageSize}", 
                    Rel = "prev", 
                    Method = "GET" 
                });
            }
            
            if (result.HasNextPage)
            {
                result.Links.Add(new Link 
                { 
                    Href = $"{baseUrl}?page={page + 1}&pageSize={pageSize}", 
                    Rel = "next", 
                    Method = "GET" 
                });
            }

            return Ok(result);
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BeaconDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um beacon pelo ID",
                          Description = "Busca e retorna informações detalhadas de um beacon específico com links HATEOAS")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDto>> GetBeacon(int id)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.ID_BEACON == id);
            
            if (entity is null) 
                return NotFound(new { 
                    message = $"Beacon com ID {id} não encontrado",
                    id = id,
                    timestamp = DateTime.UtcNow
                });
            
            var dto = _mapper.Map<BeaconDto>(entity);
            
            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/beacons";
            
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "self", Method = "GET" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "edit", Method = "PUT" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{id}", Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new Link { Href = baseUrl, Rel = "all", Method = "GET" });
            
            // Link para a moto associada
            dto.Links.Add(new Link 
            { 
                Href = $"{Request.Scheme}://{Request.Host}/api/motos/{entity.ID_MOTO}", 
                Rel = "moto", 
                Method = "GET" 
            });
            
            return Ok(dto);
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(typeof(PagedResult<BeaconDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca beacons por moto com paginação")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<PagedResult<BeaconDto>>> GetBeaconsByMoto(
            int motoId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) return BadRequest("Página deve ser maior que 0");
            if (pageSize < 1 || pageSize > 100) return BadRequest("PageSize deve estar entre 1 e 100");
            
            var totalCount = await _context.Beacons.Where(b => b.ID_MOTO == motoId).CountAsync();
            
            var entities = await _context.Beacons
                .AsNoTracking()
                .Where(b => b.ID_MOTO == motoId)
                .OrderBy(b => b.ID_BEACON)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<BeaconDto>>(entities);
            
            return Ok(new PagedResult<BeaconDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalCount
            });
        }

        // ================================
        // SUBSTITUIR ESTE MÉTODO:
        // ================================
        [HttpGet("uuid/{uuid}")]
        [ProducesResponseType(typeof(BeaconDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca beacon pelo UUID",
                          Description = "Localiza um beacon usando seu UUID como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDto>> GetBeaconByUuid(string uuid)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.UUID == uuid);
            
            if (entity is null) 
                return NotFound(new { 
                    message = $"Beacon com UUID '{uuid}' não encontrado",
                    uuid = uuid,
                    timestamp = DateTime.UtcNow
                });
            
            var dto = _mapper.Map<BeaconDto>(entity);
            
            // Adicionar links HATEOAS
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/beacons";
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_BEACON}", Rel = "self", Method = "GET" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_BEACON}", Rel = "edit", Method = "PUT" });
            dto.Links.Add(new Link { Href = $"{baseUrl}/{entity.ID_BEACON}", Rel = "delete", Method = "DELETE" });
            dto.Links.Add(new Link { Href = baseUrl, Rel = "all", Method = "GET" });
            
            return Ok(dto);
        }

        // Os outros métodos ficam iguais por enquanto
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo beacon",
                          Description = "Cria um novo registro de beacon no sistema")]
        [SwaggerRequestExample(typeof(CreateBeaconDto), typeof(BeaconRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<BeaconDto>> PostBeacon([FromBody] CreateBeaconDto dto)
        {
            var entity = new Beacon
            {
                UUID             = dto.UUID,
                BATERIA          = dto.BATERIA,
                ID_MOTO          = dto.ID_MOTO,
                ID_MODELO_BEACON = dto.ID_MODELO_BEACON
            };

            _context.Beacons.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BeaconDto>(entity);
            return CreatedAtAction(nameof(GetBeacon), new { id = entity.ID_BEACON }, result);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um beacon",
                          Description = "Atualiza informações de um beacon existente no sistema")]
        [SwaggerRequestExample(typeof(UpdateBeaconDto), typeof(BeaconRequestExample))]
        public async Task<ActionResult<BeaconDto>> PutBeacon(int id, [FromBody] UpdateBeaconDto dto)
        {
            var entity = await _context.Beacons.FirstOrDefaultAsync(b => b.ID_BEACON == id);
            if (entity is null) return NotFound();

            if (dto.UUID is not null)               entity.UUID             = dto.UUID;
            if (dto.BATERIA.HasValue)               entity.BATERIA          = dto.BATERIA.Value;
            if (dto.ID_MOTO.HasValue)               entity.ID_MOTO          = dto.ID_MOTO.Value;
            if (dto.ID_MODELO_BEACON.HasValue)      entity.ID_MODELO_BEACON = dto.ID_MODELO_BEACON.Value;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<BeaconDto>(entity));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um beacon",
                          Description = "Remove permanentemente um beacon do sistema")]
        public async Task<IActionResult> DeleteBeacon(int id)
        {
            var entity = await _context.Beacons.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Beacons.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}