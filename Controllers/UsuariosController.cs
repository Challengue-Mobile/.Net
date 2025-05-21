using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using API_.Net.Examples;
using Microsoft.AspNetCore.Http;

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de usuários do sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui usuários")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os usuários cadastrados
        /// </summary>
        /// <returns>Lista de usuários</returns>
        /// <response code="200">Retorna a lista de usuários</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os usuários",
            Description = "Obtém uma lista de todos os usuários cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuariosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        /// <summary>
        /// Obtém um usuário específico pelo ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns>Dados do usuário solicitado</returns>
        /// <response code="200">Retorna o usuário</response>
        /// <response code="404">Se o usuário não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um usuário pelo ID",
            Description = "Busca e retorna informações detalhadas de um usuário específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuarioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        /// <summary>
        /// Busca usuários por tipo
        /// </summary>
        /// <param name="tipoId">ID do tipo de usuário</param>
        /// <returns>Lista de usuários do tipo especificado</returns>
        /// <response code="200">Retorna a lista de usuários do tipo</response>
        [HttpGet("tipo/{tipoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca usuários por tipo",
            Description = "Obtém todos os usuários de um tipo específico (administrador, operador, etc.)")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UsuariosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosByTipo(int tipoId)
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.ID_TIPO_USUARIO == tipoId)
                .ToListAsync();
                
            return usuarios;
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="usuario">Dados do usuário a ser cadastrado</param>
        /// <returns>Usuário cadastrado com seu ID</returns>
        /// <response code="201">Retorna o usuário recém criado</response>
        /// <response code="400">Se os dados do usuário são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Usuarios
        ///     {
        ///        "nome": "João da Silva",
        ///        "senha": "senha123",
        ///        "email": "joao@exemplo.com",
        ///        "id_tipo_usuario": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo usuário",
            Description = "Cria um novo registro de usuário no sistema")]
        [SwaggerRequestExample(typeof(Usuario), typeof(UsuarioRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(UsuarioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.DATA_CADASTRO = DateTime.Now;
            
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.ID_USUARIO }, usuario);
        }

        /// <summary>
        /// Atualiza os dados de um usuário existente
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado</param>
        /// <param name="usuario">Novos dados do usuário</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o usuário foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o usuário não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um usuário",
            Description = "Atualiza informações de um usuário existente no sistema")]
        [SwaggerRequestExample(typeof(Usuario), typeof(UsuarioUpdateRequestExample))]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.ID_USUARIO)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Remove um usuário do sistema
        /// </summary>
        /// <param name="id">ID do usuário a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o usuário foi removido com sucesso</response>
        /// <response code="404">Se o usuário não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um usuário",
            Description = "Remove permanentemente um usuário do sistema")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.ID_USUARIO == id);
        }
    }
}