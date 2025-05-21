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
    /// API para gerenciamento de modelos de beacon
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui modelos de beacon")]
    public class ModelosBeaconController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosBeaconController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os modelos de beacon cadastrados
        /// </summary>
        /// <returns>Lista de modelos de beacon</returns>
        /// <response code="200">Retorna a lista de modelos de beacon</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os modelos de beacon",
            Description = "Obtém uma lista de todos os modelos de beacon cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ModelosBeaconListResponseExample))]
        public async Task<ActionResult<IEnumerable<ModeloBeacon>>> GetModelosBeacon()
        {
            return Ok(await _context.ModelosBeacon.ToListAsync());
        }

        /// <summary>
        /// Obtém um modelo de beacon específico pelo ID
        /// </summary>
        /// <param name="id">ID do modelo de beacon</param>
        /// <returns>Dados do modelo de beacon solicitado</returns>
        /// <response code="200">Retorna o modelo de beacon</response>
        /// <response code="404">Se o modelo de beacon não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um modelo de beacon pelo ID",
            Description = "Busca e retorna informações detalhadas de um modelo de beacon específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ModeloBeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<ModeloBeacon>> GetModeloBeacon(int id)
        {
            var modeloBeacon = await _context.ModelosBeacon.FindAsync(id);

            if (modeloBeacon == null)
            {
                return NotFound();
            }

            return modeloBeacon;
        }

        /// <summary>
        /// Cadastra um novo modelo de beacon
        /// </summary>
        /// <param name="modeloBeacon">Dados do modelo de beacon a ser cadastrado</param>
        /// <returns>Modelo de beacon cadastrado com seu ID</returns>
        /// <response code="201">Retorna o modelo de beacon recém criado</response>
        /// <response code="400">Se os dados do modelo de beacon são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/ModelosBeacon
        ///     {
        ///        "nome": "TrackBeacon Pro",
        ///        "fabricante": "TrackTech"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo modelo de beacon",
            Description = "Cria um novo registro de modelo de beacon no sistema")]
        [SwaggerRequestExample(typeof(ModeloBeacon), typeof(ModeloBeaconRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ModeloBeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<ModeloBeacon>> PostModeloBeacon(ModeloBeacon modeloBeacon)
        {
            _context.ModelosBeacon.Add(modeloBeacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModeloBeacon", new { id = modeloBeacon.ID_MODELO_BEACON }, modeloBeacon);
        }

        /// <summary>
        /// Atualiza os dados de um modelo de beacon existente
        /// </summary>
        /// <param name="id">ID do modelo de beacon a ser atualizado</param>
        /// <param name="modeloBeacon">Novos dados do modelo de beacon</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o modelo de beacon foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o modelo de beacon não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um modelo de beacon",
            Description = "Atualiza informações de um modelo de beacon existente no sistema")]
        [SwaggerRequestExample(typeof(ModeloBeacon), typeof(ModeloBeaconRequestExample))]
        public async Task<IActionResult> PutModeloBeacon(int id, ModeloBeacon modeloBeacon)
        {
            if (id != modeloBeacon.ID_MODELO_BEACON)
            {
                return BadRequest();
            }

            _context.Entry(modeloBeacon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloBeaconExists(id))
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
        /// Remove um modelo de beacon do sistema
        /// </summary>
        /// <param name="id">ID do modelo de beacon a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o modelo de beacon foi removido com sucesso</response>
        /// <response code="404">Se o modelo de beacon não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um modelo de beacon",
            Description = "Remove permanentemente um modelo de beacon do sistema")]
        public async Task<IActionResult> DeleteModeloBeacon(int id)
        {
            var modeloBeacon = await _context.ModelosBeacon.FindAsync(id);
            if (modeloBeacon == null)
            {
                return NotFound();
            }

            _context.ModelosBeacon.Remove(modeloBeacon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloBeaconExists(int id)
        {
            return _context.ModelosBeacon.Any(e => e.ID_MODELO_BEACON == id);
        }
    }
}