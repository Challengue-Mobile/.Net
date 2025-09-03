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
using API_.Net.DTOs;               // MovimentacaoDTO
using API_.Net.DTOs.Requests;      // CreateMovimentacaoDTO / UpdateMovimentacaoDTO
// using Swashbuckle.AspNetCore.Filters;
// using API_.Net.Examples;

namespace API.Net.Controllers
{
    /// <summary>API para gerenciamento de movimentações de motos</summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [SwaggerTag("Cria, lê, atualiza e exclui movimentações de motos")]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public MovimentacoesController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper  = mapper;
        }

        /// <summary>Obtém todas as movimentações registradas</summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as movimentações",
                          Description = "Obtém todas as movimentações registradas no sistema")]
        public async Task<ActionResult<IEnumerable<MovimentacaoDTO>>> GetMovimentacoes()
        {
            var entities = await _context.Movimentacoes
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MovimentacaoDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Obtém uma movimentação específica pelo ID</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma movimentação pelo ID",
                          Description = "Retorna detalhes de uma movimentação específica")]
        public async Task<ActionResult<MovimentacaoDTO>> GetMovimentacao(int id)
        {
            var entity = await _context.Movimentacoes
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MOVIMENTACAO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<MovimentacaoDTO>(entity));
        }

        /// <summary>Busca movimentações de uma moto específica</summary>
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca movimentações por moto",
                          Description = "Histórico de movimentações de uma moto, mais recentes primeiro")]
        public async Task<ActionResult<IEnumerable<MovimentacaoDTO>>> GetMovimentacoesByMoto(int motoId)
        {
            var entities = await _context.Movimentacoes
                                         .AsNoTracking()
                                         .Where(m => m.ID_MOTO == motoId)
                                         .OrderByDescending(m => m.DATA_MOVIMENTACAO)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MovimentacaoDTO>>(entities);
            return Ok(dtos);
        }

        /// <summary>Registra uma nova movimentação</summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Registra uma nova movimentação",
                          Description = "Cria um registro de movimentação de moto")]
        public async Task<ActionResult<MovimentacaoDTO>> PostMovimentacao([FromBody] CreateMovimentacaoDTO dto)
        {
            var entity = _mapper.Map<Movimentacao>(dto);
            entity.DATA_MOVIMENTACAO = DateTime.Now;

            _context.Movimentacoes.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<MovimentacaoDTO>(entity);
            return CreatedAtAction(nameof(GetMovimentacao), new { id = entity.ID_MOVIMENTACAO }, result);
        }

        /// <summary>Atualiza os dados de uma movimentação existente</summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma movimentação",
                          Description = "Atualiza informações de uma movimentação existente")]
        public async Task<ActionResult<MovimentacaoDTO>> PutMovimentacao(int id, [FromBody] UpdateMovimentacaoDTO dto)
        {
            var entity = await _context.Movimentacoes.FirstOrDefaultAsync(m => m.ID_MOVIMENTACAO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<MovimentacaoDTO>(entity));
        }

        /// <summary>Remove uma movimentação do sistema</summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Exclui uma movimentação",
                          Description = "Remove permanentemente uma movimentação do sistema")]
        public async Task<IActionResult> DeleteMovimentacao(int id)
        {
            var entity = await _context.Movimentacoes.FindAsync(id);
            if (entity is null) return NotFound();

            _context.Movimentacoes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentacaoExists(int id) =>
            _context.Movimentacoes.Any(e => e.ID_MOVIMENTACAO == id);
    }
}
