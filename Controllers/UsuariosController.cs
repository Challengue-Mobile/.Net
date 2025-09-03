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
using API_.Net.DTOs;               // UsuarioDTO
using API_.Net.DTOs.Requests;      // CreateUsuarioDTO / UpdateUsuarioDTO
// using Swashbuckle.AspNetCore.Filters;


namespace API_.Net.Controllers
{
    /// <summary>API para gerenciamento de usuários do sistema</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui usuários")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuariosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os usuários cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os usuários", Description = "Obtém todos os usuários cadastrados no sistema")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var entities = await _context.Usuarios
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um usuário específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um usuário pelo ID", Description = "Retorna um usuário específico")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var entity = await _context.Usuarios
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(u => u.ID_USUARIO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<UsuarioDto>(entity));
        }

        /// <summary>Busca usuários por tipo</summary>
        [HttpGet("tipo/{tipoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca usuários por tipo", Description = "Obtém todos os usuários de um tipo específico")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuariosByTipo(int tipoId)
        {
            var entities = await _context.Usuarios
                                         .AsNoTracking()
                                         .Where(u => u.ID_TIPO_USUARIO == tipoId)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(entities);
            return Ok(dtos);
        }

        /// <summary>Cadastra um novo usuário</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo usuário", Description = "Cria um novo registro de usuário no sistema")]
        public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] CreateUsuarioDTO dto)
        {
            var entity = _mapper.Map<Usuario>(dto);
            entity.DATA_CADASTRO = DateTime.Now;

            _context.Usuarios.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UsuarioDto>(entity);
            return CreatedAtAction(nameof(GetUsuario), new { id = entity.ID_USUARIO }, result);
        }

        /// <summary>Atualiza os dados de um usuário existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um usuário", Description = "Atualiza informações de um usuário existente")]
        public async Task<ActionResult<UsuarioDto>> PutUsuario(int id, [FromBody] UpdateUsuarioDTO dto)
        {
            var entity = await _context.Usuarios.FirstOrDefaultAsync(u => u.ID_USUARIO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity); // aplica somente os campos enviados (se usar .IgnoreNulls no profile)
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UsuarioDto>(entity));
        }

        /// <summary>Remove um usuário do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um usuário", Description = "Remove permanentemente um usuário do sistema")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var entity = await _context.Usuarios.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Usuarios.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id) =>
            _context.Usuarios.Any(e => e.ID_USUARIO == id);
    }
}
