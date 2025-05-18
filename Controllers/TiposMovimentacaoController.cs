

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
    public class TiposMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TiposMovimentacaoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/TiposMovimentacao
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoMovimentacao>>> GetTiposMovimentacao()
        {
            return Ok(await _context.TiposMovimentacao.ToListAsync());
        }

        // GET: api/TiposMovimentacao/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoMovimentacao>> GetTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.TiposMovimentacao.FindAsync(id);

            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            return tipoMovimentacao;
        }

        // POST: api/TiposMovimentacao
        [HttpPost]
        public async Task<ActionResult<TipoMovimentacao>> PostTipoMovimentacao(TipoMovimentacao tipoMovimentacao)
        {
            _context.TiposMovimentacao.Add(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoMovimentacao", new { id = tipoMovimentacao.ID_TIPO_MOVIMENTACAO }, tipoMovimentacao);
        }

        // PUT: api/TiposMovimentacao/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoMovimentacao(int id, TipoMovimentacao tipoMovimentacao)
        {
            if (id != tipoMovimentacao.ID_TIPO_MOVIMENTACAO)
            {
                return BadRequest();
            }

            _context.Entry(tipoMovimentacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimentacaoExists(id))
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

        // DELETE: api/TiposMovimentacao/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoMovimentacao(int id)
        {
            var tipoMovimentacao = await _context.TiposMovimentacao.FindAsync(id);
            if (tipoMovimentacao == null)
            {
                return NotFound();
            }

            _context.TiposMovimentacao.Remove(tipoMovimentacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoMovimentacaoExists(int id)
        {
            return _context.TiposMovimentacao.Any(e => e.ID_TIPO_MOVIMENTACAO == id);
        }
    }
}