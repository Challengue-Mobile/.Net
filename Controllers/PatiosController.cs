using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatiosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PatiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Patios
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Patio>>> GetPatios()
        {
            return await _context.Patios.ToListAsync();
        }

        // GET: api/Patios/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Patio>> GetPatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);

            if (patio == null)
            {
                return NotFound();
            }

            return patio;
        }

        // GET: api/Patios/5/Zonas
        [HttpGet("{id}/Zonas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Zona>>> GetPatioZonas(int id)
        {
            if (!PatioExists(id))
            {
                return NotFound();
            }

            var zonas = await _context.Zonas
                .Where(z => z.PatioId == id)
                .ToListAsync();

            return zonas;
        }

        // POST: api/Patios
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Patio>> PostPatio(Patio patio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Patios.Add(patio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patio);
        }

        // PUT: api/Patios/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPatio(int id, Patio patio)
        {
            if (id != patio.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(patio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatioExists(id))
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

        // DELETE: api/Patios/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePatio(int id)
        {
            var patio = await _context.Patios.FindAsync(id);
            if (patio == null)
            {
                return NotFound();
            }

            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatioExists(int id)
        {
            return _context.Patios.Any(e => e.Id == id);
        }
    }
}
