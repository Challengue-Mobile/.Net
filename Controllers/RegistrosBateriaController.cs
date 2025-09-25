using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MottothTracking.Data;
using MottothTracking.Models;

namespace MottothTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosBateriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RegistrosBateriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RegistrosBateria
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosBateria()
        {
            return await _context.RegistrosBateria
                .Include(r => r.Beacon)
                .ToListAsync();
        }

        // GET: api/RegistrosBateria/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RegistroBateria>> GetRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria
                .Include(r => r.Beacon)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registroBateria == null)
            {
                return NotFound();
            }

            return registroBateria;
        }

        // GET: api/RegistrosBateria/ByBeacon/{beaconId}
        [HttpGet("ByBeacon/{beaconId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RegistroBateria>>> GetRegistrosBateriaByBeacon(int beaconId)
        {
            return await _context.RegistrosBateria
                .Where(r => r.BeaconId == beaconId)
                .OrderByDescending(r => r.DataHora)
                .ToListAsync();
        }

        // POST: api/RegistrosBateria
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegistroBateria>> PostRegistroBateria(RegistroBateria registroBateria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registroBateria.DataHora == default)
            {
                registroBateria.DataHora = DateTime.Now;
            }

            _context.RegistrosBateria.Add(registroBateria);
            await _context.SaveChangesAsync();

            // Atualiza o nível de bateria no beacon
            var beacon = await _context.Beacons.FindAsync(registroBateria.BeaconId);
            if (beacon != null)
            {
                beacon.Bateria = registroBateria.NivelBateria;
                _context.Entry(beacon).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetRegistroBateria), new { id = registroBateria.Id }, registroBateria);
        }

        // PUT: api/RegistrosBateria/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutRegistroBateria(int id, RegistroBateria registroBateria)
        {
            if (id != registroBateria.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(registroBateria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroBateriaExists(id))
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

        // DELETE: api/RegistrosBateria/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRegistroBateria(int id)
        {
            var registroBateria = await _context.RegistrosBateria.FindAsync(id);
            if (registroBateria == null)
            {
                return NotFound();
            }

            _context.RegistrosBateria.Remove(registroBateria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroBateriaExists(int id)
        {
            return _context.RegistrosBateria.Any(e => e.Id == id);
        }
    }
}
