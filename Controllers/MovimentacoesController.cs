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

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Lista todas as movimentações",
                          Description = "Obtém todas as movimentações registradas no sistema")]
        public async Task<ActionResult<IEnumerable<MovimentacaoDto>>> GetMovimentacoes()
        {
            var entities = await _context.Movimentacoes
                                         .AsNoTracking()
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MovimentacaoDto>>(entities);
            return Ok(dtos);
        }

        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Obtém uma movimentação pelo ID",
                          Description = "Retorna detalhes de uma movimentação específica")]
        public async Task<ActionResult<MovimentacaoDto>> GetMovimentacao(int id)
        {
            var entity = await _context.Movimentacoes
                                       .AsNoTracking()
                                       .FirstOrDefaultAsync(m => m.ID_MOVIMENTACAO == id);

            if (entity is null) return NotFound();
            return Ok(_mapper.Map<MovimentacaoDto>(entity));
        }

        
        [HttpGet("moto/{motoId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Busca movimentações por moto",
                          Description = "Histórico de movimentações de uma moto, mais recentes primeiro")]
        public async Task<ActionResult<IEnumerable<MovimentacaoDto>>> GetMovimentacoesByMoto(int motoId)
        {
            var entities = await _context.Movimentacoes
                                         .AsNoTracking()
                                         .Where(m => m.ID_MOTO == motoId)
                                         .OrderByDescending(m => m.DATA_MOVIMENTACAO)
                                         .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MovimentacaoDto>>(entities);
            return Ok(dtos);
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Registra uma nova movimentação",
                          Description = "Cria um registro de movimentação de moto")]
        public async Task<ActionResult<MovimentacaoDto>> PostMovimentacao([FromBody] CreateMovimentacaoDto dto)
        {
            var entity = _mapper.Map<Movimentacao>(dto);
            entity.DATA_MOVIMENTACAO = DateTime.Now;

            _context.Movimentacoes.Add(entity);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<MovimentacaoDto>(entity);
            return CreatedAtAction(nameof(GetMovimentacao), new { id = entity.ID_MOVIMENTACAO }, result);
        }

        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Atualiza uma movimentação",
                          Description = "Atualiza informações de uma movimentação existente")]
        public async Task<ActionResult<MovimentacaoDto>> PutMovimentacao(int id, [FromBody] UpdateMovimentacaoDto dto)
        {
            var entity = await _context.Movimentacoes.FirstOrDefaultAsync(m => m.ID_MOVIMENTACAO == id);
            if (entity is null) return NotFound();

            _mapper.Map(dto, entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<MovimentacaoDto>(entity));
        }

        
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
