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
using API_.Net.DTOs;               
using API_.Net.DTOs.Requests;      


namespace API_.Net.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui motos")]
    public class MotosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MotosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as motos",
                          Description = "Obtém uma lista de todas as motos cadastradas no sistema")]
        public async Task<ActionResult<IEnumerable<MotoDto>>> GetMotos()
        {
            var entities = await _context.Motos
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MotoDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma moto pelo ID",
                          Description = "Busca e retorna informações detalhadas de uma moto específica")]
        public async Task<ActionResult<MotoDto>> GetMoto(int id)
        {
            var entity = await _context.Motos
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MOTO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<MotoDto>(entity));
        }

       
        [HttpGet("cliente/{clienteId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca motos por cliente",
                          Description = "Obtém todas as motos associadas a um cliente específico")]
        public async Task<ActionResult<IEnumerable<MotoDto>>> GetMotosByCliente(int clienteId)
        {
            var entities = await _context.Motos
                                         .AsNoTracking()
                                         .Where(m => m.ID_CLIENTE == clienteId)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MotoDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("placa/{placa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Busca moto pela placa",
                          Description = "Localiza uma moto usando sua placa como critério de busca")]
        public async Task<ActionResult<MotoDto>> GetMotoByPlaca(string placa)
        {
            var entity = await _context.Motos
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.PLACA == placa);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<MotoDto>(entity));
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra uma nova moto",
                          Description = "Cria um novo registro de moto no sistema")]
        public async Task<ActionResult<MotoDto>> PostMoto([FromBody] CreateMotoDto dto)
        {
            var entity = _mapper.Map<Moto>(dto);
            entity.DATA_REGISTRO = DateTime.Now;

            _context.Motos.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<MotoDto>(entity);
            return CreatedAtAction(nameof(GetMoto), new { id = entity.ID_MOTO }, result);
        }

      
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma moto",
                          Description = "Atualiza informações de uma moto existente no sistema")]
        public async Task<ActionResult<MotoDto>> PutMoto(int id, [FromBody] UpdateMotoDto dto)
        {
            var entity = await _context.Motos.FirstOrDefaultAsync(m => m.ID_MOTO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<MotoDto>(entity));
        }

        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma moto",
                          Description = "Remove permanentemente uma moto do sistema")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var entity = await _context.Motos.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Motos.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MotoExists(int id) =>
            _context.Motos.Any(e => e.ID_MOTO == id);
    }
}
