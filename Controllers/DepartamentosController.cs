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
using API_.Net.DTOs;               // DepartamentoDTO
using API_.Net.DTOs.Requests;      // CreateDepartamentoDto / UpdateDepartamentoDto

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de departamentos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui departamentos")]
    public class DepartamentosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DepartamentosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>
        /// Lista todos os departamentos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os departamentos",
            Description = "Obtém todos os departamentos cadastrados")]
        public async Task<ActionResult<IEnumerable<DepartamentoDTO>>> GetDepartamentos()
        {
            var entities = await _context.Departamentos
                                         .AsNoTracking()
                                         .Include(d => d.Filial)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<DepartamentoDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>
        /// Obtém um departamento pelo ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um departamento por ID",
            Description = "Retorna informações detalhadas de um departamento específico")]
        public async Task<ActionResult<DepartamentoDTO>> GetDepartamento(int id)
        {
            var entity = await _context.Departamentos
                                       .AsNoTracking()
                                       .Include(d => d.Filial)
                                       .FirstOrDefaultAsync(d => d.ID_DEPARTAMENTO == id);

            if (entity is null) return NotFound();

            return Ok(_mapper.Map<DepartamentoDTO>(entity));
        }

        /// <summary>
        /// Cria um novo departamento
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo departamento",
            Description = "Cria um registro de departamento")]
        // TODO: atualizar Examples para usar CreateDepartamentoDto
        public async Task<ActionResult<DepartamentoDTO>> PostDepartamento([FromBody] CreateDepartamentoDto dto)
        {
            var entity = _mapper.Map<Departamento>(dto);

            _context.Departamentos.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<DepartamentoDTO>(entity);
            return CreatedAtAction(nameof(GetDepartamento), new { id = entity.ID_DEPARTAMENTO }, result);
        }

        /// <summary>
        /// Atualiza um departamento existente
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um departamento",
            Description = "Atualiza informações de um departamento existente")]
        // TODO: atualizar Examples para usar UpdateDepartamentoDto
        public async Task<ActionResult<DepartamentoDTO>> PutDepartamento(int id, [FromBody] UpdateDepartamentoDto dto)
        {
            var entity = await _context.Departamentos.FirstOrDefaultAsync(d => d.ID_DEPARTAMENTO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<DepartamentoDTO>(entity));
        }

        /// <summary>
        /// Exclui um departamento
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um departamento",
            Description = "Remove um departamento do sistema")]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var entity = await _context.Departamentos.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Departamentos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DepartamentoExists(int id) =>
            _context.Departamentos.Any(e => e.ID_DEPARTAMENTO == id);
    }
}
