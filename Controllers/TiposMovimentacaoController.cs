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
using API_.Net.DTOs;               // TipoMovimentacaoDTO
using API_.Net.DTOs.Requests;      // CreateTipoMovimentacaoDTO / UpdateTipoMovimentacaoDTO

namespace API.Net.Controllers
{
    
    [Route("api/v{version:apiVersion}/[controller]")] 
    [ApiVersion("1.0")] 
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui tipos de movimentação")]
    public class TiposMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TiposMovimentacaoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os tipos de movimentação", Description = "Obtém todos os tipos cadastrados")]
        public async Task<ActionResult<IEnumerable<TipoMovimentacaoDto>>> GetTiposMovimentacao()
        {
            var entities = await _context.TiposMovimentacao
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TipoMovimentacaoDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um tipo por ID", Description = "Retorna um tipo de movimentação específico")]
        public async Task<ActionResult<TipoMovimentacaoDto>> GetTipoMovimentacao(int id)
        {
            var entity = await _context.TiposMovimentacao
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(t => t.ID_TIPO_MOVIMENTACAO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<TipoMovimentacaoDto>(entity));
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo tipo de movimentação", Description = "Cria um novo registro de tipo")]
        public async Task<ActionResult<TipoMovimentacaoDto>> PostTipoMovimentacao([FromBody] CreateTipoMovimentacaoDto dto)
        {
            var entity = _mapper.Map<TipoMovimentacao>(dto);

            _context.TiposMovimentacao.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<TipoMovimentacaoDto>(entity);
            return CreatedAtAction(nameof(GetTipoMovimentacao), new { id = entity.ID_TIPO_MOVIMENTACAO }, result);
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um tipo de movimentação", Description = "Atualiza informações de um tipo existente")]
        public async Task<ActionResult<TipoMovimentacaoDto>> PutTipoMovimentacao(int id, [FromBody] UpdateTipoMovimentacaoDto dto)
        {
            var entity = await _context.TiposMovimentacao.FirstOrDefaultAsync(t => t.ID_TIPO_MOVIMENTACAO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TipoMovimentacaoDto>(entity));
        }

        
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui um tipo de movimentação", Description = "Remove permanentemente um tipo")]
        public async Task<IActionResult> DeleteTipoMovimentacao(int id)
        {
            var entity = await _context.TiposMovimentacao.FindAsync(id);
            if (entity is null) return NotFound();

            _context.TiposMovimentacao.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
