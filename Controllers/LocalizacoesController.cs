using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizacoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LocalizacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Localizacoes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoes()
        {
            return await _context.Localizacoes
                .Include(l => l.Moto)
                .Include(l => l.Zona)
                .ToListAsync();
        }

        // GET: api/Localizacoes/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Localizacao>> GetLocalizacao(int id)
        {
            var localizacao = await _context.Localizacoes
                .Include(l => l.Moto)
                .Include(l => l.Zona)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (localizacao == null)
            {
                return NotFound();
            }

            return localizacao;
        }

        // GET: api/Localizacoes/ByMoto/{motoId}
        [HttpGet("ByMoto/{motoId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoesByMoto(int motoId)
        {
            return await _context.Localizacoes
                .Include(l => l.Zona)
                .Where(l => l.MotoId == motoId)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }

        // GET: api/Localizacoes/ByZona/{zonaId}
        [HttpGet("ByZona/{zonaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Localizacao>>> GetLocalizacoesByZona(int zonaId)
        {
            return await _context.Localizacoes
                .Include(l => l.Moto)
                .Where(l => l.ZonaId == zonaId)
                .OrderByDescending(l => l.DataHora)
                .ToListAsync();
        }

        // POST: api/Localizacoes
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Localizacao>> PostLocalizacao(Localizacao localizacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (localizacao.DataHora == default)
            {
                localizacao.DataHora = DateTime.Now;
            }

            _context.Localizacoes.Add(localizacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocalizacao), new { id = localizacao.Id }, localizacao);
        }

        // PUT: api/Localizacoes/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutLocalizacao(int id, Localizacao localizacao)
        {
            if (id != localizacao.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            return _context.Localizacoes.Any(e => e.Id == id);
        }
    }
}
