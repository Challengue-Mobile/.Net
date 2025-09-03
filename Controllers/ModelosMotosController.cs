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
using API_.Net.DTOs;               // ModeloMotoDTO
using API_.Net.DTOs.Requests;      // CreateModeloMotoDto / UpdateModeloMotoDto
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples; // ← migrar para DTOs e reativar depois

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de modelos de motos</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui modelos de motos")]
    public class ModelosMotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ModelosMotosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os modelos de motos cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os modelos de motos",
            Description = "Obtém uma lista de todos os modelos de motos cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<ModeloMotoDTO>>> GetModelosMotos()
        {
            var entities = await _context.ModelosMotos
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ModeloMotoDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um modelo de moto específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um modelo de moto pelo ID",
            Description = "Busca e retorna informações detalhadas de um modelo de moto específico")]
        public async Task<ActionResult<ModeloMotoDTO>> GetModeloMoto(int id)
        {
            var entity = await _context.ModelosMotos
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MODELO_MOTO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<ModeloMotoDTO>(entity));
        }

        /// <summary>Cadastra um novo modelo de moto</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo modelo de moto",
            Description = "Cria um novo registro de modelo de moto no sistema")]
        public async Task<ActionResult<ModeloMotoDTO>> PostModeloMoto([FromBody] CreateModeloMotoDto dto)
        {
            var entity = _mapper.Map<ModeloMoto>(dto);

            _context.ModelosMotos.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<ModeloMotoDTO>(entity);
            return CreatedAtAction(nameof(GetModeloMoto), new { id = entity.ID_MODELO_MOTO }, result);
        }

        /// <summary>Atualiza os dados de um modelo de moto existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um modelo de moto",
            Description = "Atualiza informações de um modelo de moto existente no sistema")]
        public async Task<ActionResult<ModeloMotoDTO>> PutModeloMoto(int id, [FromBody] UpdateModeloMotoDto dto)
        {
            var entity = await _context.ModelosMotos.FirstOrDefaultAsync(m => m.ID_MODELO_MOTO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ModeloMotoDTO>(entity));
        }

        /// <summary>Remove um modelo de moto do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um modelo de moto",
            Description = "Remove permanentemente um modelo de moto do sistema")]
        public async Task<IActionResult> DeleteModeloMoto(int id)
        {
            var entity = await _context.ModelosMotos.FindAsync(id);
            if (entity is null) return NotFound();

            _context.ModelosMotos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloMotoExists(int id) =>
            _context.ModelosMotos.Any(e => e.ID_MODELO_MOTO == id);
    }
}
