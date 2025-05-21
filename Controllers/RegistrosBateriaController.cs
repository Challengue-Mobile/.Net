using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using API_.Net.Examples;
using Microsoft.AspNetCore.Http;

namespace API.Net.Controllers
{
    /// <summary>
    /// API para gerenciamento de registros de bateria de beacons
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui registros de nível de bateria")]
    public class RegistrosBateriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RegistrosBateriaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os registros de bateria
        /// </summary>
        /// <returns>Lista de registros de bateria</returns>
        /// <response code="200">Retorna a lista de registros de bateria</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os registros de bateria",
            Description = "Obtém uma lista de todos os registros de nível de bateria cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegistrosBateriaListResponseExample))]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosBateria()
        {
            return Ok(await _context.RegistrosBateria.ToListAsync());
        }

        /// <summary>
        /// Obtém um registro de bateria específico pelo ID
        /// </summary>
        /// <param name="id">ID do registro de bateria</param>
        /// <returns>Dados do registro de bateria solicitado</returns>
        /// <response code="200">Retorna o registro de bateria</response>
        /// <response code="404">Se o registro de bateria não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um registro de bateria pelo ID",
            Description = "Busca e retorna informações detalhadas de um registro de bateria específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegistroBateriaResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<RegistroBateria>> GetRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria.FindAsync(id);

            if (registroBateria == null)
            {
                return NotFound();
            }

            return registroBateria;
        }

        /// <summary>
        /// Busca registros de bateria de um beacon específico
        /// </summary>
        /// <param name="beaconId">ID do beacon</param>
        /// <returns>Lista de registros de bateria do beacon, ordenados pela data mais recente</returns>
        /// <response code="200">Retorna a lista de registros de bateria do beacon</response>
        [HttpGet("beacon/{beaconId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca registros de bateria por beacon",
            Description = "Obtém o histórico de níveis de bateria de um beacon específico, ordenados do mais recente para o mais antigo")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegistrosBateriaListResponseExample))]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosByBeacon(int beaconId)
        {
            return await _context.RegistrosBateria
                .Where(rb => rb.ID_BEACON == beaconId)
                .OrderByDescending(rb => rb.DATA_HORA)
                .ToListAsync();
        }

        /// <summary>
        /// Registra um novo nível de bateria
        /// </summary>
        /// <param name="registroBateria">Dados do registro de bateria a ser cadastrado</param>
        /// <returns>Registro de bateria cadastrado com seu ID</returns>
        /// <response code="201">Retorna o registro de bateria recém criado</response>
        /// <response code="400">Se os dados do registro de bateria são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/RegistrosBateria
        ///     {
        ///        "nivel_bateria": 75,
        ///        "id_beacon": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Registra um novo nível de bateria",
            Description = "Cria um novo registro de nível de bateria para um beacon no sistema")]
        [SwaggerRequestExample(typeof(RegistroBateria), typeof(RegistroBateriaRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(RegistroBateriaResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<RegistroBateria>> PostRegistroBateria(RegistroBateria registroBateria)
        {
            registroBateria.DATA_HORA = DateTime.Now;
            
            _context.RegistrosBateria.Add(registroBateria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegistroBateria", new { id = registroBateria.ID_REGISTRO }, registroBateria);
        }

        /// <summary>
        /// Atualiza os dados de um registro de bateria existente
        /// </summary>
        /// <param name="id">ID do registro de bateria a ser atualizado</param>
        /// <param name="registroBateria">Novos dados do registro de bateria</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o registro de bateria foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o registro de bateria não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um registro de bateria",
            Description = "Atualiza informações de um registro de bateria existente no sistema")]
        [SwaggerRequestExample(typeof(RegistroBateria), typeof(RegistroBateriaRequestExample))]
        public async Task<IActionResult> PutRegistroBateria(int id, RegistroBateria registroBateria)
        {
            if (id != registroBateria.ID_REGISTRO)
            {
                return BadRequest();
            }

            _context.Entry(registroBateria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroBateriaExists(id))
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
        /// Remove um registro de bateria do sistema
        /// </summary>
        /// <param name="id">ID do registro de bateria a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o registro de bateria foi removido com sucesso</response>
        /// <response code="404">Se o registro de bateria não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um registro de bateria",
            Description = "Remove permanentemente um registro de bateria do sistema")]
        public async Task<IActionResult> DeleteRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria.FindAsync(id);
            if (registroBateria == null)
            {
                return NotFound();
            }

            _context.RegistrosBateria.Remove(registroBateria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroBateriaExists(int id)
        {
            return _context.RegistrosBateria.Any(e => e.ID_REGISTRO == id);
        }
    }
}