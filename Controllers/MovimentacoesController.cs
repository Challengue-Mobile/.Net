using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MovimentacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Movimentacoes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoes()
        {
            return await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.ZonaOrigem)
                .Include(m => m.ZonaDestino)
                .ToListAsync();
        }

        // GET: api/Movimentacoes/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Movimentacao>> GetMovimentacao(int id)
        {
            var movimentacao = await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.ZonaOrigem)
                .Include(m => m.ZonaDestino)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movimentacao == null)
            {
                return NotFound();
            }

            return movimentacao;
        }

        // GET: api/Movimentacoes/ByMoto/{motoId}
        [HttpGet("ByMoto/{motoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoesByMoto(int motoId)
        {
            return await _context.Movimentacoes
                .Include(m => m.ZonaOrigem)
                .Include(m => m.ZonaDestino)
                .Where(m => m.MotoId == motoId)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();
        }

        // GET: api/Movimentacoes/ByTipo/{tipo}
        [HttpGet("ByTipo/{tipo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Movimentacao>>> GetMovimentacoesByTipo(string tipo)
        {
            return await _context.Movimentacoes
                .Include(m => m.Moto)
                .Include(m => m.ZonaOrigem)
                .Include(m => m.ZonaDestino)
                .Where(m => m.TipoMovimentacao == tipo)
                .OrderByDescending(m => m.DataMovimentacao)
                .ToListAsync();
        }

        // POST: api/Movimentacoes
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Movimentacao>> PostMovimentacao(Movimentacao movimentacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movimentacao.DataMovimentacao == default)
            {
                movimentacao.DataMovimentacao = DateTime.Now;
            }

            _context.Movimentacoes.Add(movimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovimentacao), new { id = movimentacao.Id }, movimentacao);
        }

        // PUT: api/Movimentacoes/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMovimentacao(int id, Movimentacao movimentacao)
        {
            if (id != movimentacao.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return _context.Movimentacoes.Any(e => e.Id == id);
        }
    }
}
