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

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de tipos de usuário
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui tipos de usuário")]
    public class TiposUsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposUsuarioController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os tipos de usuário cadastrados
        /// </summary>
        /// <returns>Lista de tipos de usuário</returns>
        /// <response code="200">Retorna a lista de tipos de usuário</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os tipos de usuário",
            Description = "Obtém uma lista de todos os tipos de usuário cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TiposUsuarioListResponseExample))]
        public async Task<ActionResult<IEnumerable<TipoUsuario>>> GetTiposUsuario()
        {
            return Ok(await _context.TiposUsuario.ToListAsync());
        }

        /// <summary>
        /// Obtém um tipo de usuário específico pelo ID
        /// </summary>
        /// <param name="id">ID do tipo de usuário</param>
        /// <returns>Dados do tipo de usuário solicitado</returns>
        /// <response code="200">Retorna o tipo de usuário</response>
        /// <response code="404">Se o tipo de usuário não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um tipo de usuário pelo ID",
            Description = "Busca e retorna informações detalhadas de um tipo de usuário específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TipoUsuarioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<TipoUsuario>> GetTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TiposUsuario.FindAsync(id);

            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return tipoUsuario;
        }

        /// <summary>
        /// Cadastra um novo tipo de usuário
        /// </summary>
        /// <param name="tipoUsuario">Dados do tipo de usuário a ser cadastrado</param>
        /// <returns>Tipo de usuário cadastrado com seu ID</returns>
        /// <response code="201">Retorna o tipo de usuário recém criado</response>
        /// <response code="400">Se os dados do tipo de usuário são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/TiposUsuario
        ///     {
        ///        "descricao": "Administrador"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo tipo de usuário",
            Description = "Cria um novo registro de tipo de usuário no sistema")]
        [SwaggerRequestExample(typeof(TipoUsuario), typeof(TipoUsuarioRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(TipoUsuarioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<TipoUsuario>> PostTipoUsuario(TipoUsuario tipoUsuario)
        {
            _context.TiposUsuario.Add(tipoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoUsuario", new { id = tipoUsuario.ID_TIPO_USUARIO }, tipoUsuario);
        }

        /// <summary>
        /// Atualiza os dados de um tipo de usuário existente
        /// </summary>
        /// <param name="id">ID do tipo de usuário a ser atualizado</param>
        /// <param name="tipoUsuario">Novos dados do tipo de usuário</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o tipo de usuário foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o tipo de usuário não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um tipo de usuário",
            Description = "Atualiza informações de um tipo de usuário existente no sistema")]
        [SwaggerRequestExample(typeof(TipoUsuario), typeof(TipoUsuarioRequestExample))]
        public async Task<IActionResult> PutTipoUsuario(int id, TipoUsuario tipoUsuario)
        {
            if (id != tipoUsuario.ID_TIPO_USUARIO)
            {
                return BadRequest();
            }

            _context.Entry(tipoUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoUsuarioExists(id))
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
        /// Remove um tipo de usuário do sistema
        /// </summary>
        /// <param name="id">ID do tipo de usuário a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o tipo de usuário foi removido com sucesso</response>
        /// <response code="404">Se o tipo de usuário não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um tipo de usuário",
            Description = "Remove permanentemente um tipo de usuário do sistema")]
        public async Task<IActionResult> DeleteTipoUsuario(int id)
        {
            var tipoUsuario = await _context.TiposUsuario.FindAsync(id);
            if (tipoUsuario == null)
            {
                return NotFound();
            }

            _context.TiposUsuario.Remove(tipoUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoUsuarioExists(int id)
        {
            return _context.TiposUsuario.Any(e => e.ID_TIPO_USUARIO == id);
        }
    }
}