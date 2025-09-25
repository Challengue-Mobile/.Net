using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZonasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ZonasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Zonas
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonas()
        {
            return await _context.Zonas.Include(z => z.Patio).ToListAsync();
        }

        // GET: api/Zonas/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Zona>> GetZona(int id)
        {
            var zona = await _context.Zonas
                .Include(z => z.Patio)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zona == null)
            {
                return NotFound();
            }

            return zona;
        }

        // GET: api/Zonas/ByPatio/{patioId}
        [HttpGet("ByPatio/{patioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Zona>>> GetZonasByPatio(int patioId)
        {
            return await _context.Zonas
                .Where(z => z.PatioId == patioId)
                .ToListAsync();
        }

        // POST: api/Zonas
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Zona>> PostZona(Zona zona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Zonas.Add(zona);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetZona), new { id = zona.Id }, zona);
        }

        // PUT: api/Zonas/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutZona(int id, Zona zona)
        {
            if (id != zona.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(zona).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZonaExists(id))
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

        // DELETE: api/Zonas/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteZona(int id)
        {
            var zona = await _context.Zonas.FindAsync(id);
            if (zona == null)
            {
                return NotFound();
            }

            _context.Zonas.Remove(zona);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZonaExists(int id)
        {
            return _context.Zonas.Any(e => e.Id == id);
        }
    }
}
