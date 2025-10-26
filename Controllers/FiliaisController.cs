using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               // FilialDTO
using API_.Net.DTOs.Requests;      // CreateFilialDTO / UpdateFilialDTO

namespace API.Net.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [ApiVersion("1.0")] 
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui filiais")]
    public class FiliaisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FiliaisController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as filiais", Description = "Obtém todas as filiais cadastradas")]
        public async Task<ActionResult<IEnumerable<FiliaisDto>>> GetFiliais()
        {
            var entities = await _context.Filiais
                                         .AsNoTracking()
                                         .Include(f => f.Patio)
                                         .Include(f => f.Departamentos)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<FiliaisDto>>(entities);
            return Ok(dtos);
        }

      
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma filial por ID", Description = "Retorna informações detalhadas de uma filial")]
        public async Task<ActionResult<FiliaisDto>> GetFilial(int id)
        {
            var entity = await _context.Filiais
                                       .AsNoTracking()
                                       .Include(f => f.Patio)
                                       .Include(f => f.Departamentos)
                                       .FirstOrDefaultAsync(f => f.ID_FILIAL == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<FiliaisDto>(entity));
        }

    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra uma nova filial", Description = "Cria um novo registro de filial")]
        public async Task<ActionResult<FiliaisDto>> PostFilial([FromBody] CreateFilialDto dto)
        {
            var entity = _mapper.Map<Filial>(dto);

            _context.Filiais.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<FiliaisDto>(entity);
            return CreatedAtAction(nameof(GetFilial), new { id = entity.ID_FILIAL }, result);
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma filial", Description = "Atualiza informações de uma filial existente")]
        public async Task<ActionResult<FiliaisDto>> PutFilial(int id, [FromBody] UpdateFilialDto dto)
        {
            var entity = await _context.Filiais.FirstOrDefaultAsync(f => f.ID_FILIAL == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity); // aplica somente os campos enviados (mapeie IgnoreNulls no profile)
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<FiliaisDto>(entity));
        }

    
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma filial", Description = "Remove uma filial do sistema")]
        public async Task<IActionResult> DeleteFilial(int id)
        {
            var entity = await _context.Filiais.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Filiais.Remove(entity);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
