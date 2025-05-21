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
    /// API para gerenciamento de pátios
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui pátios")]
    public class PatiosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PatiosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os pátios cadastrados
        /// </summary>
        /// <returns>Lista de pátios</returns>
        /// <response code="200">Retorna a lista de pátios</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os pátios",
            Description = "Obtém uma lista de todos os pátios cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PatiosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            return Ok(await _context.Patios.ToListAsync());
        }

        /// <summary>
        /// Obtém um pátio específico pelo ID
        /// </summary>
        /// <param name="id">ID do pátio</param>
        /// <returns>Dados do pátio solicitado</returns>
        /// <response code="200">Retorna o pátio</response>
        /// <response code="404">Se o pátio não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um pátio pelo ID",
            Description = "Busca e retorna informações detalhadas de um pátio específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PatioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Patio>> GetPatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);

            if (patio == null)
            {
                return NotFound();
            }

            return patio;
        }

        /// <summary>
        /// Busca pátios por logradouro
        /// </summary>
        /// <param name="logradouroId">ID do logradouro</param>
        /// <returns>Lista de pátios localizados no logradouro especificado</returns>
        /// <response code="200">Retorna a lista de pátios do logradouro</response>
        [HttpGet("logradouro/{logradouroId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca pátios por logradouro",
            Description = "Obtém todos os pátios localizados em um logradouro específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PatiosListResponseExample))]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatiosByLogradouro(int logradouroId)
        {
            return await _context.Patios
                .Where(p => p.ID_LOGRADOURO == logradouroId)
                .ToListAsync();
        }

        /// <summary>
        /// Cadastra um novo pátio
        /// </summary>
        /// <param name="patio">Dados do pátio a ser cadastrado</param>
        /// <returns>Pátio cadastrado com seu ID</returns>
        /// <response code="201">Retorna o pátio recém criado</response>
        /// <response code="400">Se os dados do pátio são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Patios
        ///     {
        ///        "nome": "Pátio Central",
        ///        "id_logradouro": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo pátio",
            Description = "Cria um novo registro de pátio no sistema")]
        [SwaggerRequestExample(typeof(Patio), typeof(PatioRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(PatioResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Patio>> PostPatio(Patio patio)
        {
            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatio", new { id = patio.ID_PATIO }, patio);
        }

        /// <summary>
        /// Atualiza os dados de um pátio existente
        /// </summary>
        /// <param name="id">ID do pátio a ser atualizado</param>
        /// <param name="patio">Novos dados do pátio</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o pátio foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o pátio não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um pátio",
            Description = "Atualiza informações de um pátio existente no sistema")]
        [SwaggerRequestExample(typeof(Patio), typeof(PatioRequestExample))]
        public async Task<IActionResult> PutPatio(int id, Patio patio)
        {
            if (id != patio.ID_PATIO)
            {
                return BadRequest();
            }

            _context.Entry(patio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatioExists(id))
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
        /// Remove um pátio do sistema
        /// </summary>
        /// <param name="id">ID do pátio a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o pátio foi removido com sucesso</response>
        /// <response code="404">Se o pátio não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um pátio",
            Description = "Remove permanentemente um pátio do sistema")]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(int id)
        {
            return _context.Patios.Any(e => e.ID_PATIO == id);
        }
    }
}