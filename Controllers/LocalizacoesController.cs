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
    public class LocalizacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocalizacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Localizacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoes()
        {
            return Ok(await _context.Localizacoes.ToListAsync());
        }

        // GET: api/Localizacoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Localizacao>> GetLocalizacao(int id)
        {
            var localizacao = await _context.Localizacoes.FindAsync(id);

            if (localizacao == null)
            {
                return NotFound();
            }

            return localizacao;
        }

        // GET: api/Localizacoes/moto/5
        [HttpGet("moto/{motoId}")]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoesByMoto(int motoId)
        {
            return await _context.Localizacoes
                .Where(l => l.ID_MOTO == motoId)
                .OrderByDescending(l => l.DATA_HORA)
                .ToListAsync();
        }

        // POST: api/Localizacoes
        [HttpPost]
        public async Task<ActionResult<Localizacao>> PostLocalizacao(Localizacao localizacao)
        {
            localizacao.DATA_HORA = DateTime.Now;
            
            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocalizacao", new { id = localizacao.ID_LOCALIZACAO }, localizacao);
        }

        // PUT: api/Localizacoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalizacao(int id, Localizacao localizacao)
        {
            if (id != localizacao.ID_LOCALIZACAO)
            {
                return BadRequest();
            }

            _context.Entry(localizacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalizacaoExists(id))
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

        // DELETE: api/Localizacoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalizacao(int id)
        {
            var localizacao = await _context.Localizacoes.FindAsync(id);
            if (localizacao == null)
            {
                return NotFound();
            }

            _context.Localizacoes.Remove(localizacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocalizacaoExists(int id)
        {
            return _context.Localizacoes.Any(e => e.ID_LOCALIZACAO == id);
        }
    }
}