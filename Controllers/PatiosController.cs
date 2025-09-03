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
using API_.Net.DTOs;               // PatioDTO
using API_.Net.DTOs.Requests;      // CreatePatioDTO / UpdatePatioDTO
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples;

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de pátios</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui pátios")]
    public class PatiosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PatiosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os pátios cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os pátios",
                          Description = "Obtém uma lista de todos os pátios cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<PatiosDto>>> GetPatios()
        {
            var entities = await _context.Patios
                                         .AsNoTracking()
                                         .Include(p => p.Logradouro)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PatiosDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um pátio específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um pátio pelo ID",
                          Description = "Busca e retorna informações detalhadas de um pátio específico")]
        public async Task<ActionResult<PatiosDto>> GetPatio(int id)
        {
            var entity = await _context.Patios
                                       .AsNoTracking()
                                       .Include(p => p.Logradouro)
                                       .FirstOrDefaultAsync(p => p.ID_PATIO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<PatiosDto>(entity));
        }

        /// <summary>Busca pátios por logradouro</summary>
        [HttpGet("logradouro/{logradouroId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca pátios por logradouro",
                          Description = "Obtém todos os pátios localizados em um logradouro específico")]
        public async Task<ActionResult<IEnumerable<PatiosDto>>> GetPatiosByLogradouro(int logradouroId)
        {
            var entities = await _context.Patios
                                         .AsNoTracking()
                                         .Where(p => p.ID_LOGRADOURO == logradouroId)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<PatiosDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Cadastra um novo pátio</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo pátio",
                          Description = "Cria um novo registro de pátio no sistema")]
        public async Task<ActionResult<PatiosDto>> PostPatio([FromBody] CreatePatioDto dto)
        {
            var entity = _mapper.Map<Patio>(dto);

            _context.Patios.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<PatiosDto>(entity);
            return CreatedAtAction(nameof(GetPatio), new { id = entity.ID_PATIO }, result);
        }

        /// <summary>Atualiza os dados de um pátio existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um pátio",
                          Description = "Atualiza informações de um pátio existente no sistema")]
        public async Task<ActionResult<PatiosDto>> PutPatio(int id, [FromBody] UpdatePatioDto dto)
        {
            var entity = await _context.Patios.FirstOrDefaultAsync(p => p.ID_PATIO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<PatiosDto>(entity));
        }

        /// <summary>Remove um pátio do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um pátio",
                          Description = "Remove permanentemente um pátio do sistema")]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var entity = await _context.Patios.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Patios.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(int id) =>
            _context.Patios.Any(e => e.ID_PATIO == id);
    }
}
