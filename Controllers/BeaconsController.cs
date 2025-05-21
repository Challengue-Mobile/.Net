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
    /// API para gerenciamento de beacons
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui beacons")]
    public class BeaconsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BeaconsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os beacons cadastrados
        /// </summary>
        /// <returns>Lista de beacons</returns>
        /// <response code="200">Retorna a lista de beacons</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Lista todos os beacons",
            Description = "Obtém uma lista de todos os beacons cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeacons()
        {
            return Ok(await _context.Beacons.ToListAsync());
        }

        /// <summary>
        /// Obtém um beacon específico pelo ID
        /// </summary>
        /// <param name="id">ID do beacon</param>
        /// <returns>Dados do beacon solicitado</returns>
        /// <response code="200">Retorna o beacon</response>
        /// <response code="404">Se o beacon não for encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Obtém um beacon pelo ID",
            Description = "Busca e retorna informações detalhadas de um beacon específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Beacon>> GetBeacon(int id)
        {
            var beacon = await _context.Beacons.FindAsync(id);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        /// <summary>
        /// Busca beacons associados a uma moto específica
        /// </summary>
        /// <param name="motoId">ID da moto</param>
        /// <returns>Lista de beacons associados à moto</returns>
        /// <response code="200">Retorna a lista de beacons da moto</response>
        [HttpGet("moto/{motoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Busca beacons por moto",
            Description = "Obtém todos os beacons associados a uma moto específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<Beacon>>> GetBeaconsByMoto(int motoId)
        {
            return await _context.Beacons
                .Where(b => b.ID_MOTO == motoId)
                .ToListAsync();
        }

        /// <summary>
        /// Busca um beacon pelo seu UUID
        /// </summary>
        /// <param name="uuid">UUID do beacon</param>
        /// <returns>Beacon correspondente ao UUID</returns>
        /// <response code="200">Retorna o beacon com o UUID especificado</response>
        /// <response code="404">Se nenhum beacon com o UUID informado for encontrado</response>
        [HttpGet("uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Busca beacon pelo UUID",
            Description = "Localiza um beacon usando seu UUID como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<Beacon>> GetBeaconByUuid(string uuid)
        {
            var beacon = await _context.Beacons
                .FirstOrDefaultAsync(b => b.UUID == uuid);

            if (beacon == null)
            {
                return NotFound();
            }

            return beacon;
        }

        /// <summary>
        /// Cadastra um novo beacon
        /// </summary>
        /// <param name="beacon">Dados do beacon a ser cadastrado</param>
        /// <returns>Beacon cadastrado com seu ID</returns>
        /// <response code="201">Retorna o beacon recém criado</response>
        /// <response code="400">Se os dados do beacon são inválidos</response>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/Beacons
        ///     {
        ///        "uuid": "550e8400-e29b-41d4-a716-446655440000",
        ///        "bateria": 85,
        ///        "id_moto": 1,
        ///        "id_modelo_beacon": 1
        ///     }
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Cadastra um novo beacon",
            Description = "Cria um novo registro de beacon no sistema")]
        [SwaggerRequestExample(typeof(Beacon), typeof(BeaconRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<Beacon>> PostBeacon(Beacon beacon)
        {
            _context.Beacons.Add(beacon);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBeacon", new { id = beacon.ID_BEACON }, beacon);
        }

        /// <summary>
        /// Atualiza os dados de um beacon existente
        /// </summary>
        /// <param name="id">ID do beacon a ser atualizado</param>
        /// <param name="beacon">Novos dados do beacon</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o beacon foi atualizado com sucesso</response>
        /// <response code="400">Se o ID na URL não corresponde ao ID no corpo</response>
        /// <response code="404">Se o beacon não for encontrado</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Atualiza um beacon",
            Description = "Atualiza informações de um beacon existente no sistema")]
        [SwaggerRequestExample(typeof(Beacon), typeof(BeaconRequestExample))]
        public async Task<IActionResult> PutBeacon(int id, Beacon beacon)
        {
            if (id != beacon.ID_BEACON)
            {
                return BadRequest();
            }

            _context.Entry(beacon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeaconExists(id))
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
        /// Remove um beacon do sistema
        /// </summary>
        /// <param name="id">ID do beacon a ser removido</param>
        /// <returns>Nenhum conteúdo</returns>
        /// <response code="204">Se o beacon foi removido com sucesso</response>
        /// <response code="404">Se o beacon não for encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
            Summary = "Exclui um beacon",
            Description = "Remove permanentemente um beacon do sistema")]
        public async Task<IActionResult> DeleteBeacon(int id)
        {
            var beacon = await _context.Beacons.FindAsync(id);
            if (beacon == null)
            {
                return NotFound();
            }

            _context.Beacons.Remove(beacon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BeaconExists(int id)
        {
            return _context.Beacons.Any(e => e.ID_BEACON == id);
        }
    }
}