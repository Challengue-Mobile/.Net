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
using AutoMapper;

using API_.Net.DTOs;              // BeaconDTO (somente leitura)
using API_.Net.DTOs.Requests;     // CreateBeaconDTO / UpdateBeaconDTO

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de beacons</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui beacons")]
    public class BeaconsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BeaconsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todos os beacons cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os beacons",
                          Description = "Obtém uma lista de todos os beacons cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<BeaconDTO>>> GetBeacons()
        {
            var entities = await _context.Beacons.AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<IEnumerable<BeaconDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um beacon específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um beacon pelo ID",
                          Description = "Busca e retorna informações detalhadas de um beacon específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDTO>> GetBeacon(int id)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.ID_BEACON == id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<BeaconDTO>(entity));
        }

        /// <summary>Busca beacons associados a uma moto específica</summary>
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca beacons por moto",
                          Description = "Obtém todos os beacons associados a uma moto específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<BeaconDTO>>> GetBeaconsByMoto(int motoId)
        {
            var entities = await _context.Beacons.AsNoTracking()
                                                 .Where(b => b.ID_MOTO == motoId)
                                                 .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BeaconDTO>>(entities));
        }

        /// <summary>Busca um beacon pelo seu UUID</summary>
        [HttpGet("uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca beacon pelo UUID",
                          Description = "Localiza um beacon usando seu UUID como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDTO>> GetBeaconByUuid(string uuid)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.UUID == uuid);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<BeaconDTO>(entity));
        }

        /// <summary>Cadastra um novo beacon</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo beacon",
                          Description = "Cria um novo registro de beacon no sistema")]
        [SwaggerRequestExample(typeof(CreateBeaconDTO), typeof(BeaconRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<BeaconDTO>> PostBeacon([FromBody] CreateBeaconDTO dto)
        {
            // opcional: var entity = _mapper.Map<Beacon>(dto);
            var entity = new Beacon
            {
                UUID             = dto.UUID,
                BATERIA          = dto.BATERIA,
                ID_MOTO          = dto.ID_MOTO,
                ID_MODELO_BEACON = dto.ID_MODELO_BEACON
            };

            _context.Beacons.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BeaconDTO>(entity);
            return CreatedAtAction(nameof(GetBeacon), new { id = entity.ID_BEACON }, result);
        }

        /// <summary>Atualiza os dados de um beacon existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um beacon",
                          Description = "Atualiza informações de um beacon existente no sistema")]
        [SwaggerRequestExample(typeof(UpdateBeaconDTO), typeof(BeaconRequestExample))]
        public async Task<ActionResult<BeaconDTO>> PutBeacon(int id, [FromBody] UpdateBeaconDTO dto)
        {
            var entity = await _context.Beacons.FirstOrDefaultAsync(b => b.ID_BEACON == id);
            if (entity is null) return NotFound();

            // _mapper.Map(dto, entity); // alternativa (se IgnoreNulls estiver no profile)
            if (dto.UUID is not null)               entity.UUID             = dto.UUID;
            if (dto.BATERIA.HasValue)               entity.BATERIA          = dto.BATERIA.Value;
            if (dto.ID_MOTO.HasValue)               entity.ID_MOTO          = dto.ID_MOTO.Value;
            if (dto.ID_MODELO_BEACON.HasValue)      entity.ID_MODELO_BEACON = dto.ID_MODELO_BEACON.Value;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<BeaconDTO>(entity));
        }

        /// <summary>Remove um beacon do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um beacon",
                          Description = "Remove permanentemente um beacon do sistema")]
        public async Task<IActionResult> DeleteBeacon(int id)
        {
            var entity = await _context.Beacons.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Beacons.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
