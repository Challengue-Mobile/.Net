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

using API_.Net.DTOs;              
using API_.Net.DTOs.Requests;     

namespace API.Net.Controllers
{

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

 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os beacons",
                          Description = "Obtém uma lista de todos os beacons cadastrados no sistema")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<BeaconDto>>> GetBeacons()
        {
            var entities = await _context.Beacons.AsNoTracking().ToListAsync();
            var dtos = _mapper.Map<IEnumerable<BeaconDto>>(entities);
            return Ok(dtos);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um beacon pelo ID",
                          Description = "Busca e retorna informações detalhadas de um beacon específico")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDto>> GetBeacon(int id)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.ID_BEACON == id);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<BeaconDto>(entity));
        }

   
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca beacons por moto",
                          Description = "Obtém todos os beacons associados a uma moto específica")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconsListResponseExample))]
        public async Task<ActionResult<IEnumerable<BeaconDto>>> GetBeaconsByMoto(int motoId)
        {
            var entities = await _context.Beacons.AsNoTracking()
                                                 .Where(b => b.ID_MOTO == motoId)
                                                 .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<BeaconDto>>(entities));
        }

     
        [HttpGet("uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca beacon pelo UUID",
                          Description = "Localiza um beacon usando seu UUID como critério de busca")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(NotFoundResponseExample))]
        public async Task<ActionResult<BeaconDto>> GetBeaconByUuid(string uuid)
        {
            var entity = await _context.Beacons.AsNoTracking()
                                               .FirstOrDefaultAsync(b => b.UUID == uuid);
            if (entity is null) return NotFound();
            return Ok(_mapper.Map<BeaconDto>(entity));
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo beacon",
                          Description = "Cria um novo registro de beacon no sistema")]
        [SwaggerRequestExample(typeof(CreateBeaconDto), typeof(BeaconRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(BeaconResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        public async Task<ActionResult<BeaconDto>> PostBeacon([FromBody] CreateBeaconDto dto)
        {
            
            var entity = new Beacon
            {
                UUID             = dto.UUID,
                BATERIA          = dto.BATERIA,
                ID_MOTO          = dto.ID_MOTO,
                ID_MODELO_BEACON = dto.ID_MODELO_BEACON
            };

            _context.Beacons.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<BeaconDto>(entity);
            return CreatedAtAction(nameof(GetBeacon), new { id = entity.ID_BEACON }, result);
        }

   
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um beacon",
                          Description = "Atualiza informações de um beacon existente no sistema")]
        [SwaggerRequestExample(typeof(UpdateBeaconDto), typeof(BeaconRequestExample))]
        public async Task<ActionResult<BeaconDto>> PutBeacon(int id, [FromBody] UpdateBeaconDto dto)
        {
            var entity = await _context.Beacons.FirstOrDefaultAsync(b => b.ID_BEACON == id);
            if (entity is null) return NotFound();

            
            if (dto.UUID is not null)               entity.UUID             = dto.UUID;
            if (dto.BATERIA.HasValue)               entity.BATERIA          = dto.BATERIA.Value;
            if (dto.ID_MOTO.HasValue)               entity.ID_MOTO          = dto.ID_MOTO.Value;
            if (dto.ID_MODELO_BEACON.HasValue)      entity.ID_MODELO_BEACON = dto.ID_MODELO_BEACON.Value;

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<BeaconDto>(entity));
        }

  
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
