using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_.Net.Models;
using API_.Net.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovimentacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Movimentacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoes()
        {
            return Ok(await _context.Movimentacoes.ToListAsync());
        }

        // GET: api/Movimentacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movimentacao>> GetMovimentacao(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);

            if (movimentacao == null)
            {
                return NotFound();
            }

            return movimentacao;
        }

        // GET: api/Movimentacoes/moto/5
        [HttpGet("moto/{motoId}")]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoesByMoto(int motoId)
        {
            return await _context.Movimentacoes
                .Where(m => m.ID_MOTO == motoId)
                .OrderByDescending(m => m.DATA_MOVIMENTACAO)
                .ToListAsync();
        }

        // POST: api/Movimentacoes
        [HttpPost]
        public async Task<ActionResult<Movimentacao>> PostMovimentacao(Movimentacao movimentacao)
        {
            movimentacao.DATA_MOVIMENTACAO = DateTime.Now;
            
            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovimentacao", new { id = movimentacao.ID_MOVIMENTACAO }, movimentacao);
        }

        // PUT: api/Movimentacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovimentacao(int id, Movimentacao movimentacao)
        {
            if (id != movimentacao.ID_MOVIMENTACAO)
            {
                return BadRequest();
            }

            _context.Entry(movimentacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentacaoExists(id))
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

        // DELETE: api/Movimentacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovimentacao(int id)
        {
            var movimentacao = await _context.Movimentacoes.FindAsync(id);
            if (movimentacao == null)
            {
                return NotFound();
            }

            _context.Movimentacoes.Remove(movimentacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovimentacaoExists(int id)
        {
            return _context.Movimentacoes.Any(e => e.ID_MOVIMENTACAO == id);
        }
    }
}