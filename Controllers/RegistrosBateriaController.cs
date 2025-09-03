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
using API_.Net.DTOs;               // RegistroBateriaDTO
using API_.Net.DTOs.Requests;      // CreateRegistroBateriaDTO / UpdateRegistroBateriaDTO
// using Swashbuckle.AspNetCore.Filters;


namespace API.Net.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui registros de nível de bateria")]
    public class RegistrosBateriaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RegistrosBateriaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os registros de bateria",
            Description = "Obtém uma lista de todos os registros de nível de bateria cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<RegistroBateriaDto>>> GetRegistrosBateria()
        {
            var entities = await _context.RegistrosBateria
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<RegistroBateriaDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um registro de bateria pelo ID",
            Description = "Busca e retorna informações detalhadas de um registro de bateria específico")]
        public async Task<ActionResult<RegistroBateriaDto>> GetRegistroBateria(int id)
        {
            var entity = await _context.RegistrosBateria
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(r => r.ID_REGISTRO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<RegistroBateriaDto>(entity));
        }

        
        [HttpGet("beacon/{beaconId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca registros de bateria por beacon",
            Description = "Obtém o histórico de níveis de bateria de um beacon, ordenado do mais recente para o mais antigo")]
        public async Task<ActionResult<IEnumerable<RegistroBateriaDto>>> GetRegistrosByBeacon(int beaconId)
        {
            var entities = await _context.RegistrosBateria
                                         .AsNoTracking()
                                         .Where(rb => rb.ID_BEACON == beaconId)
                                         .OrderByDescending(rb => rb.DATA_HORA)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<RegistroBateriaDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registra um novo nível de bateria",
            Description = "Cria um novo registro de nível de bateria para um beacon")]
        public async Task<ActionResult<RegistroBateriaDto>> PostRegistroBateria([FromBody] CreateRegistroBateriaDto dto)
        {
            var entity = _mapper.Map<RegistroBateria>(dto);
            entity.DATA_HORA = DateTime.Now;

            _context.RegistrosBateria.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<RegistroBateriaDto>(entity);
            return CreatedAtAction(nameof(GetRegistroBateria), new { id = entity.ID_REGISTRO }, result);
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um registro de bateria",
            Description = "Atualiza informações de um registro de bateria existente")]
        public async Task<ActionResult<RegistroBateriaDto>> PutRegistroBateria(int id, [FromBody] UpdateRegistroBateriaDto dto)
        {
            var entity = await _context.RegistrosBateria.FirstOrDefaultAsync(r => r.ID_REGISTRO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<RegistroBateriaDto>(entity));
        }

       
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um registro de bateria",
            Description = "Remove permanentemente um registro de bateria do sistema")]
        public async Task<IActionResult> DeleteRegistroBateria(int id)
        {
            var entity = await _context.RegistrosBateria.FindAsync(id);
            if (entity is null) return NotFound();

            _context.RegistrosBateria.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroBateriaExists(int id) =>
            _context.RegistrosBateria.Any(e => e.ID_REGISTRO == id);
    }
}
