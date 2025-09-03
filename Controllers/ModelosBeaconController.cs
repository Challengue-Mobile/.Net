using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               // ModeloBeaconDTO
using API_.Net.DTOs.Requests;      // CreateModeloBeaconDTO / UpdateModeloBeaconDTO
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples;

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de modelos de beacon</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui modelos de beacon")]
    public class ModelosBeaconController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ModelosBeaconController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os modelos de beacon cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os modelos de beacon",
            Description = "Obtém uma lista de todos os modelos de beacon cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<ModeloBeaconDto>>> GetModelosBeacon()
        {
            var entities = await _context.ModelosBeacon
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ModeloBeaconDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um modelo de beacon específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um modelo de beacon pelo ID",
            Description = "Busca e retorna informações detalhadas de um modelo de beacon específico")]
        public async Task<ActionResult<ModeloBeaconDto>> GetModeloBeacon(int id)
        {
            var entity = await _context.ModelosBeacon
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MODELO_BEACON == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<ModeloBeaconDto>(entity));
        }

        /// <summary>Cadastra um novo modelo de beacon</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo modelo de beacon",
            Description = "Cria um novo registro de modelo de beacon no sistema")]
        public async Task<ActionResult<ModeloBeaconDto>> PostModeloBeacon([FromBody] CreateModeloBeaconDTO dto)
        {
            var entity = _mapper.Map<ModeloBeacon>(dto);

            _context.ModelosBeacon.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ModeloBeaconDto>(entity);
            return CreatedAtAction(nameof(GetModeloBeacon), new { id = entity.ID_MODELO_BEACON }, result);
        }

        /// <summary>Atualiza os dados de um modelo de beacon existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um modelo de beacon",
            Description = "Atualiza informações de um modelo de beacon existente no sistema")]
        public async Task<ActionResult<ModeloBeaconDto>> PutModeloBeacon(int id, [FromBody] UpdateModeloBeaconDTO dto)
        {
            var entity = await _context.ModelosBeacon.FirstOrDefaultAsync(m => m.ID_MODELO_BEACON == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ModeloBeaconDto>(entity));
        }

        /// <summary>Remove um modelo de beacon do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um modelo de beacon",
            Description = "Remove permanentemente um modelo de beacon do sistema")]
        public async Task<IActionResult> DeleteModeloBeacon(int id)
        {
            var entity = await _context.ModelosBeacon.FindAsync(id);
            if (entity is null) return NotFound();

            _context.ModelosBeacon.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloBeaconExists(int id) =>
            _context.ModelosBeacon.Any(e => e.ID_MODELO_BEACON == id);
    }
}
