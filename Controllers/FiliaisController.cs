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
using API_.Net.DTOs;               // FilialDTO
using API_.Net.DTOs.Requests;      // CreateFilialDto / UpdateFilialDto

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de filiais</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui filiais")]
    public class FiliaisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FiliaisController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Lista todas as filiais</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as filiais", Description = "Obtém todas as filiais cadastradas")]
        public async Task<ActionResult<IEnumerable<FilialDTO>>> GetFiliais()
        {
            var entities = await _context.Filiais
                                         .AsNoTracking()
                                         .Include(f => f.Patio)
                                         .Include(f => f.Departamentos)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<FilialDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém uma filial pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma filial por ID", Description = "Retorna informações detalhadas de uma filial")]
        public async Task<ActionResult<FilialDTO>> GetFilial(int id)
        {
            var entity = await _context.Filiais
                                       .AsNoTracking()
                                       .Include(f => f.Patio)
                                       .Include(f => f.Departamentos)
                                       .FirstOrDefaultAsync(f => f.ID_FILIAL == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<FilialDTO>(entity));
        }

        /// <summary>Cadastra uma nova filial</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra uma nova filial", Description = "Cria um novo registro de filial")]
        public async Task<ActionResult<FilialDTO>> PostFilial([FromBody] CreateFilialDto dto)
        {
            var entity = _mapper.Map<Filial>(dto);

            _context.Filiais.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<FilialDTO>(entity);
            return CreatedAtAction(nameof(GetFilial), new { id = entity.ID_FILIAL }, result);
        }

        /// <summary>Atualiza uma filial existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma filial", Description = "Atualiza informações de uma filial existente")]
        public async Task<ActionResult<FilialDTO>> PutFilial(int id, [FromBody] UpdateFilialDto dto)
        {
            var entity = await _context.Filiais.FirstOrDefaultAsync(f => f.ID_FILIAL == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<FilialDTO>(entity));
        }

        /// <summary>Exclui uma filial</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma filial", Description = "Remove uma filial do sistema")]
        public async Task<IActionResult> DeleteFilial(int id)
        {
            var entity = await _context.Filiais.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Filiais.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
