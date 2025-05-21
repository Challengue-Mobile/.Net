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
    /// API para gerenciamento de modelos de motos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui modelos de motos")]
    public class ModelosMotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelosMotosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os modelos de motos cadastrados
        /// </summary>
        /// <returns>Lista de modelos de motos</returns>
        /// <response code="200">Retorna a lista de modelos de motos</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os modelos de motos",
            Description = "Obtém uma lista de todos os modelos de motos cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ModelosMotosListResponseExample))]
        public async Task<ActionResult<IEnumerable<ModeloMoto>>> GetModelosMotos()
        {
            return Ok(await _context.ModelosMotos.ToListAsync());
        }

        /// <summary>
        /// Obtém um modelo de moto específico pelo ID
        /// </summary>
        /// <param name="id">ID do modelo de moto</param>
        /// <returns>Dados do modelo de moto solicitado</returns>
        /// <response code="200">Retorna o modelo de moto</response>
        /// <response code="404">Se o modelo de moto não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um modelo de moto pelo ID",
            Description = "Busca e retorna informações detalhadas de um modelo de moto específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ModeloMotoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<ModeloMoto>> GetModeloMoto(int id)
        {
            var modeloMoto = await _context.ModelosMotos.FindAsync(id);

            if (modeloMoto == null)
            {
                return NotFound();
            }

            return modeloMoto;
        }

        /// <summary>
        /// Cadastra um novo modelo de moto
        /// </summary>
        /// <param name="modeloMoto">Dados do modelo de moto a ser cadastrado</param>
        /// <returns>Modelo de moto cadastrado com seu ID</returns>
        /// <response code="201">Retorna o modelo de moto recém criado</response>
        /// <response code="400">Se os dados do modelo de moto são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/ModelosMotos
        ///     {
        ///        "nome": "CB 500",
        ///        "fabricante": "Honda"
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo modelo de moto",
            Description = "Cria um novo registro de modelo de moto no sistema")]
        [SwaggerRequestExample(typeof(ModeloMoto), typeof(ModeloMotoRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ModeloMotoResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<ModeloMoto>> PostModeloMoto(ModeloMoto modeloMoto)
        {
            _context.ModelosMotos.Add(modeloMoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModeloMoto", new { id = modeloMoto.ID_MODELO_MOTO }, modeloMoto);
        }

        /// <summary>
        /// Atualiza os dados de um modelo de moto existente
        /// </summary>
        /// <param name="id">ID do modelo de moto a ser atualizado</param>
        /// <param name="modeloMoto">Novos dados do modelo de moto</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o modelo de moto foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o modelo de moto não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um modelo de moto",
            Description = "Atualiza informações de um modelo de moto existente no sistema")]
        [SwaggerRequestExample(typeof(ModeloMoto), typeof(ModeloMotoRequestExample))]
        public async Task<IActionResult> PutModeloMoto(int id, ModeloMoto modeloMoto)
        {
            if (id != modeloMoto.ID_MODELO_MOTO)
            {
                return BadRequest();
            }

            _context.Entry(modeloMoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModeloMotoExists(id))
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
        /// Remove um modelo de moto do sistema
        /// </summary>
        /// <param name="id">ID do modelo de moto a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o modelo de moto foi removido com sucesso</response>
        /// <response code="404">Se o modelo de moto não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um modelo de moto",
            Description = "Remove permanentemente um modelo de moto do sistema")]
        public async Task<IActionResult> DeleteModeloMoto(int id)
        {
            var modeloMoto = await _context.ModelosMotos.FindAsync(id);
            if (modeloMoto == null)
            {
                return NotFound();
            }

            _context.ModelosMotos.Remove(modeloMoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModeloMotoExists(int id)
        {
            return _context.ModelosMotos.Any(e => e.ID_MODELO_MOTO == id);
        }
    }
}