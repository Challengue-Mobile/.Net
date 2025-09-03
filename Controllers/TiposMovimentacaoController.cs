using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using API_.Net.Data;
using API_.Net.Models;
using AutoMapper;
using API_.Net.DTOs;               // TipoMovimentacaoDTO
using API_.Net.DTOs.Requests;      // CreateTipoMovimentacaoDTO / UpdateTipoMovimentacaoDTO

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de tipos de movimentação</summary>
    [Route("api/[controller]")]
    [ApiController]
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

        /// <summary>Obtém todos os tipos de movimentação cadastrados</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todos os tipos de movimentação", Description = "Obtém todos os tipos cadastrados")]
        public async Task<ActionResult<IEnumerable<TipoMovimentacaoDTO>>> GetTiposMovimentacao()
        {
            var entities = await _context.TiposMovimentacao
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<TipoMovimentacaoDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém um tipo de movimentação específico pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém um tipo por ID", Description = "Retorna um tipo de movimentação específico")]
        public async Task<ActionResult<TipoMovimentacaoDTO>> GetTipoMovimentacao(int id)
        {
            var entity = await _context.TiposMovimentacao
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(t => t.ID_TIPO_MOVIMENTACAO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<TipoMovimentacaoDTO>(entity));
        }

        /// <summary>Cadastra um novo tipo de movimentação</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Cadastra um novo tipo de movimentação", Description = "Cria um novo registro de tipo")]
        public async Task<ActionResult<TipoMovimentacaoDTO>> PostTipoMovimentacao([FromBody] CreateTipoMovimentacaoDTO dto)
        {
            var entity = _mapper.Map<TipoMovimentacao>(dto);

            _context.TiposMovimentacao.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<TipoMovimentacaoDTO>(entity);
            return CreatedAtAction(nameof(GetTipoMovimentacao), new { id = entity.ID_TIPO_MOVIMENTACAO }, result);
        }

        /// <summary>Atualiza os dados de um tipo de movimentação existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza um tipo de movimentação", Description = "Atualiza informações de um tipo existente")]
        public async Task<ActionResult<TipoMovimentacaoDTO>> PutTipoMovimentacao(int id, [FromBody] UpdateTipoMovimentacaoDTO dto)
        {
            var entity = await _context.TiposMovimentacao.FirstOrDefaultAsync(t => t.ID_TIPO_MOVIMENTACAO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TipoMovimentacaoDTO>(entity));
        }

        /// <summary>Remove um tipo de movimentação do sistema</summary>
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
