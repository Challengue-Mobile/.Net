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
using API_.Net.DTOs;               // TipoUsuarioDTO
using API_.Net.DTOs.Requests;      // CreateTipoUsuarioDto / UpdateTipoUsuarioDto
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples; // ← migre seus Examples para DTOs e reative depois, se quiser

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de tipos de usuário</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui tipos de usuário")]
    public class TiposUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TiposUsuarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os tipos de usuário cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os tipos de usuário",
            Description = "Obtém uma lista de todos os tipos de usuário cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<TipoUsuarioDTO>>> GetTiposUsuario()
        {
            var entities = await _context.TiposUsuario
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TipoUsuarioDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um tipo de usuário específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um tipo de usuário pelo ID",
            Description = "Busca e retorna informações detalhadas de um tipo de usuário específico")]
        public async Task<ActionResult<TipoUsuarioDTO>> GetTipoUsuario(int id)
        {
            var entity = await _context.TiposUsuario
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(t => t.ID_TIPO_USUARIO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<TipoUsuarioDTO>(entity));
        }

        /// <summary>Cadastra um novo tipo de usuário</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo tipo de usuário",
            Description = "Cria um novo registro de tipo de usuário no sistema")]
        public async Task<ActionResult<TipoUsuarioDTO>> PostTipoUsuario([FromBody] CreateTipoUsuarioDto dto)
        {
            var entity = _mapper.Map<TipoUsuario>(dto);

            _context.TiposUsuario.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<TipoUsuarioDTO>(entity);
            return CreatedAtAction(nameof(GetTipoUsuario), new { id = entity.ID_TIPO_USUARIO }, result);
        }

        /// <summary>Atualiza os dados de um tipo de usuário existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um tipo de usuário",
            Description = "Atualiza informações de um tipo de usuário existente no sistema")]
        public async Task<ActionResult<TipoUsuarioDTO>> PutTipoUsuario(int id, [FromBody] UpdateTipoUsuarioDto dto)
        {
            var entity = await _context.TiposUsuario.FirstOrDefaultAsync(t => t.ID_TIPO_USUARIO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TipoUsuarioDTO>(entity));
        }

        /// <summary>Remove um tipo de usuário do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um tipo de usuário",
            Description = "Remove permanentemente um tipo de usuário do sistema")]
        public async Task<IActionResult> DeleteTipoUsuario(int id)
        {
            var entity = await _context.TiposUsuario.FindAsync(id);
            if (entity is null) return NotFound();

            _context.TiposUsuario.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoUsuarioExists(int id) =>
            _context.TiposUsuario.Any(e => e.ID_TIPO_USUARIO == id);
    }
}
